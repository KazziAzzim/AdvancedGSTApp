using AdvancedGSTApp.Services.Interfaces;

namespace AdvancedGSTApp.Services.SaaS;

public class TenantAccessService(ITenantContext tenantContext) : ITenantAccessService
{
    public Task<bool> CanAccessTenantAsync(int tenantId) => Task.FromResult(tenantContext.IsSuperAdmin || tenantContext.TenantId == tenantId);

    public async Task EnsureTenantAccessAsync(int tenantId)
    {
        if (!await CanAccessTenantAsync(tenantId))
        {
            throw new UnauthorizedAccessException("You cannot access another tenant's data.");
        }
    }
}
