using AdvancedGSTApp.Models;

namespace AdvancedGSTApp.Services.Interfaces;

public interface ITenantService
{
    Task<Tenant> CreateTenantAsync(Tenant tenant, string adminEmail, string adminPassword);
    Task<bool> ActivateAsync(int tenantId);
    Task<bool> DeactivateAsync(int tenantId);
}
