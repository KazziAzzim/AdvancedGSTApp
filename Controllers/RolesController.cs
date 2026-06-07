using AdvancedGSTApp.Data;
using AdvancedGSTApp.Filters;
using AdvancedGSTApp.Models;
using AdvancedGSTApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace AdvancedGSTApp.Controllers;

[Authorize]
[PermissionAuthorize("Role Management", "View")]
public class RolesController(
    RoleManager<ApplicationRole> roleManager,
    UserManager<ApplicationUser> userManager,
    ApplicationDbContext db) : Controller
{
    public async Task<IActionResult> Index(string? search)
    {
        var roles = roleManager.Roles.AsQueryable();
        if (!string.IsNullOrWhiteSpace(search)) roles = roles.Where(r => r.Name!.Contains(search) || (r.Description != null && r.Description.Contains(search)));
        var roleList = await roles.OrderBy(r => r.Name).ToListAsync();
        var items = new List<RoleListItemViewModel>();
        foreach (var role in roleList)
        {
            items.Add(new RoleListItemViewModel { Id = role.Id, RoleName = role.Name ?? string.Empty, Description = role.Description, IsActive = role.IsActive, CreatedDate = role.CreatedDate, UserCount = (await userManager.GetUsersInRoleAsync(role.Name!)).Count });
        }
        return View(new RoleListViewModel { Roles = items, Search = search });
    }

    [PermissionAuthorize("Role Management", "Create")]
    public IActionResult Create() => View(new CreateRoleViewModel());

    [HttpPost, ValidateAntiForgeryToken]
    [PermissionAuthorize("Role Management", "Create")]
    public async Task<IActionResult> Create(CreateRoleViewModel model)
    {
        await ValidateUniqueRoleAsync(model.RoleName);
        if (!ModelState.IsValid) return View(model);
        var role = new ApplicationRole { Name = model.RoleName.Trim(), Description = model.Description, IsActive = model.IsActive, CreatedDate = DateTime.UtcNow };
        var result = await roleManager.CreateAsync(role);
        if (result.Succeeded)
        {
            await SeedMissingPermissionsAsync(role);
            await AddAuditAsync("Role created", role.Id, $"Created role {role.Name}.");
            TempData["Success"] = "Role created successfully.";
            return RedirectToAction(nameof(Index));
        }
        AddIdentityErrors(result); return View(model);
    }

    public async Task<IActionResult> Details(string id)
    {
        var role = await roleManager.FindByIdAsync(id);
        if (role is null) return NotFound();
        return View(new RoleDetailsViewModel { Id = role.Id, RoleName = role.Name ?? string.Empty, Description = role.Description, IsActive = role.IsActive, CreatedDate = role.CreatedDate, UpdatedDate = role.UpdatedDate, UserCount = (await userManager.GetUsersInRoleAsync(role.Name!)).Count });
    }

    [PermissionAuthorize("Role Management", "Edit")]
    public async Task<IActionResult> Edit(string id)
    {
        var role = await roleManager.FindByIdAsync(id);
        if (role is null) return NotFound();
        return View(new EditRoleViewModel { Id = role.Id, RoleName = role.Name ?? string.Empty, Description = role.Description, IsActive = role.IsActive });
    }

    [HttpPost, ValidateAntiForgeryToken]
    [PermissionAuthorize("Role Management", "Edit")]
    public async Task<IActionResult> Edit(EditRoleViewModel model)
    {
        var role = await roleManager.FindByIdAsync(model.Id);
        if (role is null) return NotFound();
        if (role.Name == PermissionCatalog.SuperAdminRole && model.RoleName != PermissionCatalog.SuperAdminRole) ModelState.AddModelError(nameof(model.RoleName), "Super Admin role name cannot be changed.");
        await ValidateUniqueRoleAsync(model.RoleName, model.Id);
        if (!ModelState.IsValid) return View(model);
        role.Name = model.RoleName.Trim(); role.Description = model.Description; role.IsActive = model.IsActive; role.UpdatedDate = DateTime.UtcNow;
        var result = await roleManager.UpdateAsync(role);
        if (result.Succeeded)
        {
            await AddAuditAsync("Role updated", role.Id, $"Updated role {role.Name}.");
            TempData["Success"] = "Role updated successfully.";
            return RedirectToAction(nameof(Index));
        }
        AddIdentityErrors(result); return View(model);
    }

    [PermissionAuthorize("Role Management", "Edit")]
    public async Task<IActionResult> ManagePermissions(string id)
    {
        var role = await roleManager.FindByIdAsync(id);
        if (role is null) return NotFound();
        await SeedMissingPermissionsAsync(role);
        var permissions = await db.RolePermissions.Where(p => p.RoleId == role.Id).OrderBy(p => p.ModuleName).ToListAsync();
        return View(new ManageRolePermissionsViewModel { RoleId = role.Id, RoleName = role.Name ?? string.Empty, Permissions = permissions.Select(ToViewModel).ToList() });
    }

    [HttpPost, ValidateAntiForgeryToken]
    [PermissionAuthorize("Role Management", "Edit")]
    public async Task<IActionResult> ManagePermissions(ManageRolePermissionsViewModel model)
    {
        var role = await roleManager.FindByIdAsync(model.RoleId);
        if (role is null) return NotFound();
        await SeedMissingPermissionsAsync(role);
        var existing = await db.RolePermissions.Where(p => p.RoleId == role.Id).ToListAsync();
        foreach (var posted in model.Permissions)
        {
            var permission = existing.FirstOrDefault(p => p.ModuleName == posted.ModuleName);
            if (permission is null) continue;
            permission.CanView = posted.CanView; permission.CanCreate = posted.CanCreate; permission.CanEdit = posted.CanEdit; permission.CanDelete = posted.CanDelete; permission.CanPrint = posted.CanPrint; permission.CanExport = posted.CanExport; permission.CanEmail = posted.CanEmail; permission.CanGenerate = posted.CanGenerate; permission.CanApprove = posted.CanApprove; permission.UpdatedDate = DateTime.UtcNow;
        }
        if (role.Name == PermissionCatalog.SuperAdminRole)
        {
            foreach (var permission in existing) SetAll(permission, true);
        }
        await AddAuditAsync("Permissions updated", role.Id, $"Updated permissions for {role.Name}.");
        await db.SaveChangesAsync();
        TempData["Success"] = "Permissions updated successfully.";
        return RedirectToAction(nameof(ManagePermissions), new { id = role.Id });
    }

    [HttpPost, ValidateAntiForgeryToken]
    [PermissionAuthorize("Role Management", "Delete")]
    public async Task<IActionResult> Delete(string id)
    {
        var role = await roleManager.FindByIdAsync(id);
        if (role is null) return NotFound();
        if (role.Name == PermissionCatalog.SuperAdminRole)
        {
            TempData["Error"] = "Super Admin role cannot be deleted.";
            return RedirectToAction(nameof(Index));
        }
        if ((await userManager.GetUsersInRoleAsync(role.Name!)).Any())
        {
            TempData["Error"] = "Cannot delete a role assigned to users.";
            return RedirectToAction(nameof(Index));
        }
        var result = await roleManager.DeleteAsync(role);
        if (result.Succeeded)
        {
            await AddAuditAsync("Role deleted", role.Id, $"Deleted role {role.Name}.");
            TempData["Success"] = "Role deleted successfully.";
            return RedirectToAction(nameof(Index));
        }
        AddIdentityErrors(result); return RedirectToAction(nameof(Index));
    }

    private async Task SeedMissingPermissionsAsync(ApplicationRole role)
    {
        var existing = await db.RolePermissions.Where(p => p.RoleId == role.Id).Select(p => p.ModuleName).ToListAsync();
        foreach (var module in PermissionCatalog.Modules.Except(existing))
        {
            var permission = new RolePermission { RoleId = role.Id, ModuleName = module };
            if (role.Name == PermissionCatalog.SuperAdminRole) SetAll(permission, true);
            db.RolePermissions.Add(permission);
        }
        await db.SaveChangesAsync();
    }

    private static RolePermissionViewModel ToViewModel(RolePermission permission) => new() { Id = permission.Id, ModuleName = permission.ModuleName, CanView = permission.CanView, CanCreate = permission.CanCreate, CanEdit = permission.CanEdit, CanDelete = permission.CanDelete, CanPrint = permission.CanPrint, CanExport = permission.CanExport, CanEmail = permission.CanEmail, CanGenerate = permission.CanGenerate, CanApprove = permission.CanApprove };
    private static void SetAll(RolePermission permission, bool value) { permission.CanView = value; permission.CanCreate = value; permission.CanEdit = value; permission.CanDelete = value; permission.CanPrint = value; permission.CanExport = value; permission.CanEmail = value; permission.CanGenerate = value; permission.CanApprove = value; }
    private async Task ValidateUniqueRoleAsync(string roleName, string? currentId = null) { if (await roleManager.Roles.AnyAsync(r => r.Id != currentId && r.NormalizedName == roleName.ToUpper())) ModelState.AddModelError(nameof(CreateRoleViewModel.RoleName), "Role name already exists."); }
    private void AddIdentityErrors(IdentityResult result) { foreach (var error in result.Errors) ModelState.AddModelError(string.Empty, error.Description); }
    private async Task AddAuditAsync(string action, string entityId, string description)
    {
        db.AuditLogs.Add(new AuditLog
        {
            ActionType = action,
            EntityName = "Role Management",
            EntityId = entityId,
            ModuleName = "Role Management",
            Description = description,
            UserId = User.FindFirstValue(ClaimTypes.NameIdentifier),
            IpAddress = HttpContext.Connection.RemoteIpAddress?.ToString()
        });
        await db.SaveChangesAsync();
    }
}
