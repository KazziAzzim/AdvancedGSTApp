using AdvancedGSTApp.Data;
using AdvancedGSTApp.Models;
using AdvancedGSTApp.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AdvancedGSTApp.Controllers;

[Authorize]
public class TenantDashboardController(ApplicationDbContext db, ITenantContext tenantContext, ISubscriptionService subscriptionService) : Controller
{
    public async Task<IActionResult> Index()
    {
        if (!tenantContext.TenantId.HasValue) return RedirectToAction("Index", "SuperAdminDashboard");
        var tenantId = tenantContext.TenantId.Value;
        var plan = await subscriptionService.GetCurrentPlanAsync(tenantId);
        var start = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, 1);
        var tenant = await db.Tenants.IgnoreQueryFilters().FirstOrDefaultAsync(x => x.Id == tenantId);
        return View(new TenantDashboardViewModel
        {
            TotalSales = await db.SalesInvoices.SumAsync(x => x.GrandTotal),
            TotalPurchases = await db.PurchaseInvoices.SumAsync(x => x.GrandTotal),
            GstPayable = await db.OutputTaxLedgers.SumAsync(x => x.CGSTCredit + x.SGSTCredit + x.IGSTCredit),
            Itc = await db.InputTaxCreditLedgers.SumAsync(x => x.CGSTCredit + x.SGSTCredit + x.IGSTCredit),
            Customers = await db.Customers.CountAsync(),
            Suppliers = await db.Suppliers.CountAsync(),
            Users = await db.Users.CountAsync(x => x.TenantId == tenantId && x.IsActive),
            CurrentPlan = plan?.PlanName ?? "No Plan",
            SubscriptionExpiryDate = tenant?.SubscriptionEndDate,
            InvoiceUsageThisMonth = await db.SalesInvoices.CountAsync(x => x.InvoiceDate >= start),
            MaxInvoicesPerMonth = plan?.MaxInvoicesPerMonth ?? 0,
            PendingGstReturns = await db.GstReturnFilings.CountAsync(x => x.FilingStatus != "Filed"),
            RecentInvoices = await db.SalesInvoices.Include(x => x.Customer).OrderByDescending(x => x.InvoiceDate).Take(5).ToListAsync()
        });
    }
}
