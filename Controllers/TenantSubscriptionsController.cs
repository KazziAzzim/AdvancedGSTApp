using AdvancedGSTApp.Data;
using AdvancedGSTApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace AdvancedGSTApp.Controllers;

[Authorize(Roles = "Super Admin,Support Admin")]
[Route("SuperAdmin/TenantSubscriptions")]
public class TenantSubscriptionsController(ApplicationDbContext db) : Controller
{
    [HttpGet("")]
    public async Task<IActionResult> Index(int? tenantId)
    {
        ViewBag.Tenants = await db.Tenants.IgnoreQueryFilters().Where(x => !x.IsDeleted).Select(x => new SelectListItem(x.BusinessName, x.Id.ToString(), x.Id == tenantId)).ToListAsync();
        var query = db.TenantSubscriptions.IgnoreQueryFilters().Include(x => x.Tenant).Include(x => x.SubscriptionPlan).AsQueryable();
        if (tenantId.HasValue) query = query.Where(x => x.TenantId == tenantId.Value);
        return View(await query.OrderByDescending(x => x.EndDate).ToListAsync());
    }

    [HttpGet("Create")]
    public async Task<IActionResult> Create()
    {
        await PopulateListsAsync();
        return View(new TenantSubscriptionViewModel { StartDate = DateTime.UtcNow.Date, EndDate = DateTime.UtcNow.Date.AddMonths(1), Status = "Active" });
    }

    [HttpPost("Create"), ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(TenantSubscriptionViewModel model)
    {
        if (!ModelState.IsValid) { await PopulateListsAsync(); return View(model); }
        var subscription = new TenantSubscription { TenantId = model.TenantId, SubscriptionPlanId = model.SubscriptionPlanId, StartDate = model.StartDate, EndDate = model.EndDate, BillingCycle = model.BillingCycle, Amount = model.Amount, Status = model.Status, PaymentStatus = model.PaymentStatus, PaymentReferenceNumber = model.PaymentReferenceNumber };
        db.TenantSubscriptions.Add(subscription);
        var tenant = await db.Tenants.IgnoreQueryFilters().FirstOrDefaultAsync(x => x.Id == model.TenantId);
        if (tenant is not null) { tenant.SubscriptionPlanId = model.SubscriptionPlanId; tenant.SubscriptionStartDate = model.StartDate; tenant.SubscriptionEndDate = model.EndDate; tenant.IsTrial = model.Status == "Trial"; tenant.IsActive = model.Status is "Active" or "Trial"; }
        await db.SaveChangesAsync(); TempData["Success"] = "Tenant subscription saved."; return RedirectToAction(nameof(Index));
    }

    private async Task PopulateListsAsync()
    {
        ViewBag.Tenants = await db.Tenants.IgnoreQueryFilters().Where(x => !x.IsDeleted).Select(x => new SelectListItem(x.BusinessName, x.Id.ToString())).ToListAsync();
        ViewBag.Plans = await db.SubscriptionPlans.Where(x => x.IsActive).Select(x => new SelectListItem(x.PlanName, x.Id.ToString())).ToListAsync();
    }
}
