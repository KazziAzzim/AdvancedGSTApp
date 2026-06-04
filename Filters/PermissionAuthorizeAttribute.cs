using AdvancedGSTApp.Data;
using AdvancedGSTApp.Models;
using AdvancedGSTApp.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace AdvancedGSTApp.Filters;

public class PermissionAuthorizeAttribute : TypeFilterAttribute
{
    public PermissionAuthorizeAttribute(string moduleName, string permissionName) : base(typeof(PermissionAuthorizeFilter))
    {
        Arguments = [moduleName, permissionName];
    }
}

public class PermissionAuthorizeFilter(
    string moduleName,
    string permissionName,
    IPermissionService permissionService,
    ApplicationDbContext db) : IAsyncAuthorizationFilter
{
    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        var userId = context.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrWhiteSpace(userId))
        {
            context.Result = new ChallengeResult();
            return;
        }

        if (await permissionService.HasPermissionAsync(userId, moduleName, permissionName))
        {
            return;
        }

        db.AuditLogs.Add(new AuditLog
        {
            ActionType = "Unauthorized Access",
            EntityName = moduleName,
            EntityId = userId,
            ModuleName = moduleName,
            Description = $"Denied {permissionName} permission for {moduleName}.",
            UserId = userId,
            IpAddress = context.HttpContext.Connection.RemoteIpAddress?.ToString()
        });
        await db.SaveChangesAsync();
        context.Result = new RedirectToActionResult("AccessDenied", "Account", null);
    }
}
