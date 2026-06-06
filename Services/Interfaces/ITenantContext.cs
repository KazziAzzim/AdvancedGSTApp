namespace AdvancedGSTApp.Services.Interfaces;

public interface ITenantContext
{
    int? TenantId { get; }
    string? TenantName { get; }
    bool IsSuperAdmin { get; }
    bool HasTenant { get; }
    Task<int?> GetCurrentTenantIdAsync();
    Task<bool> IsCurrentUserSuperAdminAsync();
}
