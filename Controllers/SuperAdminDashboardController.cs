using AdvancedGSTApp.Data;
using AdvancedGSTApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AdvancedGSTApp.Controllers;

[Authorize(Roles = "Super Admin,Support Admin")]
[Route("SuperAdmin/Dashboard")]
public class SuperAdminDashboardController(ApplicationDbContext db) : Controller
{
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var today = DateTime.UtcNow.Date;
        var startOfMonth = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, 1);
        var tenants = db.Tenants.IgnoreQueryFilters().Where(x => !x.IsDeleted);
        var model = new PlatformDashboardViewModel
        {
            TotalTenants = await tenants.CountAsync(),
            ActiveTenants = await tenants.CountAsync(x => x.IsActive),
            TrialTenants = await tenants.CountAsync(x => x.IsTrial),
            ExpiredTenants = await tenants.CountAsync(x => x.SubscriptionEndDate != null && x.SubscriptionEndDate < today),
            MonthlyRecurringRevenue = await db.TenantSubscriptions.IgnoreQueryFilters().Where(x => x.Status == "Active").SumAsync(x => x.Amount),
            TotalPaymentsReceived = await db.SaasPayments.IgnoreQueryFilters().Where(x => x.PaymentStatus == "Paid").SumAsync(x => x.Amount),
            RecentTenantRegistrations = await tenants.OrderByDescending(x => x.CreatedDate).Take(5).ToListAsync(),
            ExpiringSubscriptions = await db.TenantSubscriptions.IgnoreQueryFilters().Include(x => x.Tenant).Include(x => x.SubscriptionPlan).Where(x => x.EndDate >= today && x.EndDate <= today.AddDays(30)).OrderBy(x => x.EndDate).Take(10).ToListAsync()
        };
        var topTenants = await tenants.Take(10).ToListAsync();
        model.TenantUsageSummary = topTenants.Select(x => new TenantUsageViewModel
        {
            TenantId = x.Id,
            BusinessName = x.BusinessName,
            UsersUsed = db.Users.Count(u => u.TenantId == x.Id && u.IsActive),
            MaxUsers = x.SubscriptionPlan != null ? x.SubscriptionPlan.MaxUsers : 0,
            InvoicesThisMonth = db.SalesInvoices.IgnoreQueryFilters().Count(i => i.TenantId == x.Id && i.InvoiceDate >= startOfMonth),
            MaxInvoicesPerMonth = x.SubscriptionPlan != null ? x.SubscriptionPlan.MaxInvoicesPerMonth : 0
        }).ToList();
        return View(model);
    }
}
