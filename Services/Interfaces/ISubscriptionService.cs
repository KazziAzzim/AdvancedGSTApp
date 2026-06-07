using AdvancedGSTApp.Models;

namespace AdvancedGSTApp.Services.Interfaces;

public interface ISubscriptionService
{
    Task<bool> CanCreateUserAsync(int tenantId);
    Task<bool> CanCreateInvoiceAsync(int tenantId);
    Task<bool> HasFeatureAsync(int tenantId, string featureName);
    Task<bool> IsSubscriptionActiveAsync(int tenantId);
    Task<SubscriptionPlan?> GetCurrentPlanAsync(int tenantId);
}
