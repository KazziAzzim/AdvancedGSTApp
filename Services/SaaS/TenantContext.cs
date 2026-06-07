using AdvancedGSTApp.Services.Interfaces;
using System.Security.Claims;

namespace AdvancedGSTApp.Services.SaaS;

public class TenantContext(IHttpContextAccessor httpContextAccessor) : ITenantContext
{
    private ClaimsPrincipal? User => httpContextAccessor.HttpContext?.User;

    public int? TenantId => int.TryParse(User?.FindFirstValue("tenant_id"), out var tenantId) ? tenantId : null;
    public string? TenantName => User?.FindFirstValue("tenant_name");
    public bool IsSuperAdmin => User?.IsInRole("Super Admin") == true || User?.FindFirstValue("is_platform_user") == "true";
    public bool HasTenant => TenantId.HasValue;
    public Task<int?> GetCurrentTenantIdAsync() => Task.FromResult(TenantId);
    public Task<bool> IsCurrentUserSuperAdminAsync() => Task.FromResult(IsSuperAdmin);
}
