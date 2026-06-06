using AdvancedGSTApp.Data;
using AdvancedGSTApp.Models;
using AdvancedGSTApp.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AdvancedGSTApp.Services;

public class PermissionService(
    ApplicationDbContext db,
    UserManager<ApplicationUser> userManager) : IPermissionService
{
    public async Task<bool> HasPermissionAsync(string userId, string moduleName, string permissionName)
    {
        if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(moduleName))
        {
            return false;
        }

        var user = await userManager.FindByIdAsync(userId);
        if (user is null || !user.IsActive)
        {
            return false;
        }

        var roles = await userManager.GetRolesAsync(user);
        if (roles.Contains(PermissionCatalog.SuperAdminRole))
        {
            return true;
        }

        var normalizedRoles = roles.Select(r => r.ToUpperInvariant()).ToList();
        var permissions = await db.RolePermissions
            .Where(permission => permission.ModuleName == moduleName &&
                db.Roles.Any(role => role.Id == permission.RoleId && role.IsActive && normalizedRoles.Contains(role.NormalizedName!)))
            .ToListAsync();

        return permissions.Any(permission => permissionName.ToLowerInvariant() switch
        {
            "view" => permission.CanView,
            "create" => permission.CanCreate,
            "edit" => permission.CanEdit,
            "delete" => permission.CanDelete,
            "print" => permission.CanPrint,
            "export" => permission.CanExport,
            "email" => permission.CanEmail,
            "generate" => permission.CanGenerate,
            "approve" => permission.CanApprove,
            _ => false
        });
    }

    public Task<bool> CanViewAsync(string userId, string moduleName) => HasPermissionAsync(userId, moduleName, "View");
    public Task<bool> CanCreateAsync(string userId, string moduleName) => HasPermissionAsync(userId, moduleName, "Create");
    public Task<bool> CanEditAsync(string userId, string moduleName) => HasPermissionAsync(userId, moduleName, "Edit");
    public Task<bool> CanDeleteAsync(string userId, string moduleName) => HasPermissionAsync(userId, moduleName, "Delete");
    public Task<bool> CanPrintAsync(string userId, string moduleName) => HasPermissionAsync(userId, moduleName, "Print");
    public Task<bool> CanExportAsync(string userId, string moduleName) => HasPermissionAsync(userId, moduleName, "Export");
    public Task<bool> CanEmailAsync(string userId, string moduleName) => HasPermissionAsync(userId, moduleName, "Email");
    public Task<bool> CanGenerateAsync(string userId, string moduleName) => HasPermissionAsync(userId, moduleName, "Generate");
    public Task<bool> CanApproveAsync(string userId, string moduleName) => HasPermissionAsync(userId, moduleName, "Approve");
}
