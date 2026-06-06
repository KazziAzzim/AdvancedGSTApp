namespace AdvancedGSTApp.Services.Interfaces;

public interface ITenantAccessService
{
    Task<bool> CanAccessTenantAsync(int tenantId);
    Task EnsureTenantAccessAsync(int tenantId);
}
