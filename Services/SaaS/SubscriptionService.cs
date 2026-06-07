using AdvancedGSTApp.Data;
using AdvancedGSTApp.Models;
using AdvancedGSTApp.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AdvancedGSTApp.Services.SaaS;

public class SubscriptionService(ApplicationDbContext db) : ISubscriptionService
{
    public async Task<bool> CanCreateUserAsync(int tenantId)
    {
        var plan = await GetCurrentPlanAsync(tenantId);
        if (plan is null || plan.MaxUsers <= 0) return true;
        var users = await db.Users.CountAsync(x => x.TenantId == tenantId && x.IsActive);
        return users < plan.MaxUsers;
    }

    public async Task<bool> CanCreateInvoiceAsync(int tenantId)
    {
        var plan = await GetCurrentPlanAsync(tenantId);
        if (plan is null || plan.MaxInvoicesPerMonth <= 0) return true;
        var start = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, 1);
        var invoices = await db.SalesInvoices.CountAsync(x => x.TenantId == tenantId && x.InvoiceDate >= start);
        return invoices < plan.MaxInvoicesPerMonth;
    }

    public async Task<bool> HasFeatureAsync(int tenantId, string featureName)
    {
        var plan = await GetCurrentPlanAsync(tenantId);
        if (plan is null) return false;
        return featureName.Trim().ToLowerInvariant() switch
        {
            "einvoice" or "e-invoice" => plan.HasEInvoice,
            "ewaybill" or "e-waybill" or "e-way bill" => plan.HasEWayBill,
            "reconciliation" or "gst reconciliation" => plan.HasGstReconciliation,
            "gstapi" or "gst api" => plan.HasGstApiIntegration,
            "advancedreports" or "advanced reports" => plan.HasAdvancedReports,
            _ => true
        };
    }

    public async Task<bool> IsSubscriptionActiveAsync(int tenantId)
    {
        var tenant = await db.Tenants.IgnoreQueryFilters().FirstOrDefaultAsync(x => x.Id == tenantId && !x.IsDeleted);
        if (tenant is null || !tenant.IsActive) return false;
        if (tenant.SubscriptionEndDate.HasValue && tenant.SubscriptionEndDate.Value.Date < DateTime.UtcNow.Date) return false;
        return await db.TenantSubscriptions.IgnoreQueryFilters().AnyAsync(x => x.TenantId == tenantId && (x.Status == "Active" || x.Status == "Trial") && x.EndDate.Date >= DateTime.UtcNow.Date);
    }

    public async Task<SubscriptionPlan?> GetCurrentPlanAsync(int tenantId)
    {
        var subscription = await db.TenantSubscriptions.IgnoreQueryFilters()
            .Include(x => x.SubscriptionPlan)
            .Where(x => x.TenantId == tenantId && (x.Status == "Active" || x.Status == "Trial"))
            .OrderByDescending(x => x.EndDate)
            .FirstOrDefaultAsync();
        return subscription?.SubscriptionPlan ?? await db.Tenants.IgnoreQueryFilters().Where(x => x.Id == tenantId).Select(x => x.SubscriptionPlan).FirstOrDefaultAsync();
    }
}
