using AdvancedGSTApp.Data;
using AdvancedGSTApp.Filters;
using AdvancedGSTApp.Models;
using AdvancedGSTApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace AdvancedGSTApp.Controllers;

[Authorize]
[PermissionAuthorize("User Management", "View")]
public class UsersController(
    UserManager<ApplicationUser> userManager,
    RoleManager<ApplicationRole> roleManager,
    ApplicationDbContext db) : Controller
{
    private const int PageSize = 10;

    public async Task<IActionResult> Index(string? search, string? roleId, bool? isActive, int page = 1)
    {
        page = Math.Max(1, page);
        var query = userManager.Users.AsQueryable();
        if (!string.IsNullOrWhiteSpace(search))
        {
            var term = search.Trim();
            query = query.Where(u => u.FullName.Contains(term) || u.Email!.Contains(term) || (u.PhoneNumber != null && u.PhoneNumber.Contains(term)));
        }
        if (isActive.HasValue) query = query.Where(u => u.IsActive == isActive.Value);
        if (!string.IsNullOrWhiteSpace(roleId))
        {
            query = from user in query
                    join userRole in db.UserRoles on user.Id equals userRole.UserId
                    where userRole.RoleId == roleId
                    select user;
        }

        var total = await query.CountAsync();
        var users = await query.OrderBy(u => u.FullName).Skip((page - 1) * PageSize).Take(PageSize).ToListAsync();
        var items = new List<UserListItemViewModel>();
        foreach (var user in users)
        {
            items.Add(new UserListItemViewModel
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email ?? string.Empty,
                PhoneNumber = user.PhoneNumber,
                Role = string.Join(", ", await userManager.GetRolesAsync(user)),
                IsActive = user.IsActive,
                LastLoginDate = user.LastLoginDate,
                CreatedDate = user.CreatedDate
            });
        }

        return View(new UserListViewModel
        {
            Users = items,
            Search = search,
            RoleId = roleId,
            IsActive = isActive,
            Page = page,
            TotalPages = (int)Math.Ceiling(total / (double)PageSize),
            Roles = await BuildRoleSelectListAsync(roleId)
        });
    }

    [PermissionAuthorize("User Management", "Create")]
    public async Task<IActionResult> Create() => View(new CreateUserViewModel { Roles = await BuildRoleSelectListAsync() });

    [HttpPost, ValidateAntiForgeryToken]
    [PermissionAuthorize("User Management", "Create")]
    public async Task<IActionResult> Create(CreateUserViewModel model)
    {
        await ValidateUniqueUserAsync(model.Email, model.UserName);
        if (!ModelState.IsValid)
        {
            model.Roles = await BuildRoleSelectListAsync(model.RoleId);
            return View(model);
        }

        var role = await roleManager.FindByIdAsync(model.RoleId);
        if (role is null || !role.IsActive)
        {
            ModelState.AddModelError(nameof(model.RoleId), "Select an active role.");
            model.Roles = await BuildRoleSelectListAsync(model.RoleId);
            return View(model);
        }

        var user = new ApplicationUser
        {
            FullName = model.FullName.Trim(),
            Email = model.Email.Trim(),
            UserName = model.UserName.Trim(),
            PhoneNumber = model.PhoneNumber,
            ProfilePhoto = model.ProfilePhoto,
            IsActive = model.IsActive,
            EmailConfirmed = true,
            CreatedDate = DateTime.UtcNow
        };
        var result = await userManager.CreateAsync(user, model.Password);
        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(user, role.Name!);
            await AddAuditAsync("User created", "User Management", user.Id, $"Created user {user.Email}.");
            TempData["Success"] = "User created successfully.";
            return RedirectToAction(nameof(Index));
        }

        AddIdentityErrors(result);
        model.Roles = await BuildRoleSelectListAsync(model.RoleId);
        return View(model);
    }

    public async Task<IActionResult> Details(string id)
    {
        var user = await FindUserAsync(id);
        if (user is null) return NotFound();
        var roleNames = await userManager.GetRolesAsync(user);
        var role = roleNames.FirstOrDefault() ?? string.Empty;
        return View(new UserDetailsViewModel
        {
            Id = user.Id, FullName = user.FullName, Email = user.Email ?? string.Empty, PhoneNumber = user.PhoneNumber,
            UserName = user.UserName ?? string.Empty, IsActive = user.IsActive, ProfilePhoto = user.ProfilePhoto, RoleName = role,
            CreatedDate = user.CreatedDate, UpdatedDate = user.UpdatedDate, LastLoginDate = user.LastLoginDate
        });
    }

    [PermissionAuthorize("User Management", "Edit")]
    public async Task<IActionResult> Edit(string id)
    {
        var user = await FindUserAsync(id);
        if (user is null) return NotFound();
        var roleName = (await userManager.GetRolesAsync(user)).FirstOrDefault();
        var roleId = roleName is null ? string.Empty : (await roleManager.FindByNameAsync(roleName))?.Id ?? string.Empty;
        return View(new EditUserViewModel
        {
            Id = user.Id, FullName = user.FullName, Email = user.Email ?? string.Empty, PhoneNumber = user.PhoneNumber,
            UserName = user.UserName ?? string.Empty, IsActive = user.IsActive, ProfilePhoto = user.ProfilePhoto,
            RoleId = roleId, Roles = await BuildRoleSelectListAsync(roleId)
        });
    }

    [HttpPost, ValidateAntiForgeryToken]
    [PermissionAuthorize("User Management", "Edit")]
    public async Task<IActionResult> Edit(EditUserViewModel model)
    {
        var user = await FindUserAsync(model.Id);
        if (user is null) return NotFound();
        await ValidateUniqueUserAsync(model.Email, model.UserName, user.Id);
        if (!await CanModifyUserAsync(user, model.IsActive)) ModelState.AddModelError(string.Empty, "Super Admin users cannot be modified or deactivated by this account.");
        if (!ModelState.IsValid)
        {
            model.Roles = await BuildRoleSelectListAsync(model.RoleId);
            return View(model);
        }

        var oldRoles = await userManager.GetRolesAsync(user);
        var role = await roleManager.FindByIdAsync(model.RoleId);
        if (role is null || !role.IsActive)
        {
            ModelState.AddModelError(nameof(model.RoleId), "Select an active role.");
            model.Roles = await BuildRoleSelectListAsync(model.RoleId);
            return View(model);
        }

        user.FullName = model.FullName.Trim(); user.Email = model.Email.Trim(); user.UserName = model.UserName.Trim();
        user.PhoneNumber = model.PhoneNumber; user.ProfilePhoto = model.ProfilePhoto; user.IsActive = model.IsActive; user.UpdatedDate = DateTime.UtcNow;
        var result = await userManager.UpdateAsync(user);
        if (result.Succeeded)
        {
            await userManager.RemoveFromRolesAsync(user, oldRoles);
            await userManager.AddToRoleAsync(user, role.Name!);
            await AddAuditAsync("User updated", "User Management", user.Id, $"Updated user {user.Email}; role changed to {role.Name}.");
            TempData["Success"] = "User updated successfully.";
            return RedirectToAction(nameof(Index));
        }
        AddIdentityErrors(result);
        model.Roles = await BuildRoleSelectListAsync(model.RoleId);
        return View(model);
    }

    [PermissionAuthorize("User Management", "Edit")]
    public async Task<IActionResult> ChangePassword(string id)
    {
        var user = await FindUserAsync(id);
        if (user is null) return NotFound();
        return View(new ChangeUserPasswordViewModel { Id = user.Id, FullName = user.FullName });
    }

    [HttpPost, ValidateAntiForgeryToken]
    [PermissionAuthorize("User Management", "Edit")]
    public async Task<IActionResult> ChangePassword(ChangeUserPasswordViewModel model)
    {
        var user = await FindUserAsync(model.Id);
        if (user is null) return NotFound();
        if (!ModelState.IsValid) { model.FullName = user.FullName; return View(model); }
        var token = await userManager.GeneratePasswordResetTokenAsync(user);
        var result = await userManager.ResetPasswordAsync(user, token, model.Password);
        if (result.Succeeded)
        {
            await AddAuditAsync("Password changed by admin", "User Management", user.Id, $"Password reset for {user.Email}.");
            TempData["Success"] = "Password changed successfully.";
            return RedirectToAction(nameof(Index));
        }
        AddIdentityErrors(result); model.FullName = user.FullName; return View(model);
    }

    [HttpPost, ValidateAntiForgeryToken]
    [PermissionAuthorize("User Management", "Edit")]
    public async Task<IActionResult> Activate(string id) => await SetActiveAsync(id, true);

    [HttpPost, ValidateAntiForgeryToken]
    [PermissionAuthorize("User Management", "Edit")]
    public async Task<IActionResult> Deactivate(string id) => await SetActiveAsync(id, false);

    [HttpPost, ValidateAntiForgeryToken]
    [PermissionAuthorize("User Management", "Delete")]
    public async Task<IActionResult> Delete(string id)
    {
        var user = await FindUserAsync(id);
        if (user is null) return NotFound();
        if (await userManager.IsInRoleAsync(user, PermissionCatalog.SuperAdminRole))
        {
            TempData["Error"] = "Super Admin cannot be deleted.";
            return RedirectToAction(nameof(Index));
        }
        user.IsActive = false; user.UpdatedDate = DateTime.UtcNow;
        await userManager.UpdateAsync(user);
        await AddAuditAsync("User deleted", "User Management", user.Id, $"Soft-deleted user {user.Email}.");
        TempData["Success"] = "User deactivated and soft deleted successfully.";
        return RedirectToAction(nameof(Index));
    }

    private async Task<IActionResult> SetActiveAsync(string id, bool active)
    {
        var user = await FindUserAsync(id);
        if (user is null) return NotFound();
        if (!active && await userManager.IsInRoleAsync(user, PermissionCatalog.SuperAdminRole))
        {
            TempData["Error"] = "Super Admin cannot be deactivated.";
            return RedirectToAction(nameof(Index));
        }
        user.IsActive = active; user.UpdatedDate = DateTime.UtcNow;
        await userManager.UpdateAsync(user);
        await AddAuditAsync(active ? "User activated" : "User deactivated", "User Management", user.Id, $"{(active ? "Activated" : "Deactivated")} {user.Email}.");
        TempData["Success"] = active ? "User activated." : "User deactivated.";
        return RedirectToAction(nameof(Index));
    }

    private Task<ApplicationUser?> FindUserAsync(string id) => userManager.Users.FirstOrDefaultAsync(u => u.Id == id);
    private async Task<List<SelectListItem>> BuildRoleSelectListAsync(string? selected = null) => await roleManager.Roles.Where(r => r.IsActive).OrderBy(r => r.Name).Select(r => new SelectListItem(r.Name, r.Id, r.Id == selected)).ToListAsync();
    private async Task ValidateUniqueUserAsync(string email, string userName, string? currentId = null)
    {
        if (await userManager.Users.AnyAsync(u => u.Id != currentId && u.NormalizedEmail == email.ToUpper())) ModelState.AddModelError(nameof(CreateUserViewModel.Email), "Email already exists.");
        if (await userManager.Users.AnyAsync(u => u.Id != currentId && u.NormalizedUserName == userName.ToUpper())) ModelState.AddModelError(nameof(CreateUserViewModel.UserName), "Username already exists.");
    }
    private async Task<bool> CanModifyUserAsync(ApplicationUser user, bool requestedActive) => requestedActive || !await userManager.IsInRoleAsync(user, PermissionCatalog.SuperAdminRole);
    private void AddIdentityErrors(IdentityResult result) { foreach (var error in result.Errors) ModelState.AddModelError(string.Empty, error.Description); }
    private async Task AddAuditAsync(string action, string module, string entityId, string description)
    {
        db.AuditLogs.Add(new AuditLog { ActionType = action, EntityName = module, EntityId = entityId, ModuleName = module, Description = description, UserId = User.FindFirstValue(ClaimTypes.NameIdentifier), IpAddress = HttpContext.Connection.RemoteIpAddress?.ToString() });
        await db.SaveChangesAsync();
    }
}
