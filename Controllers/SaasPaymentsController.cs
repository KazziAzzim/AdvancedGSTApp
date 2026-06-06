using AdvancedGSTApp.Data;
using AdvancedGSTApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace AdvancedGSTApp.Controllers;

[Authorize(Roles = "Super Admin,Support Admin")]
[Route("SuperAdmin/SaasPayments")]
public class SaasPaymentsController(ApplicationDbContext db) : Controller
{
    [HttpGet("")]
    public async Task<IActionResult> Index(int? tenantId)
    {
        ViewBag.Tenants = await db.Tenants.IgnoreQueryFilters().Where(x => !x.IsDeleted).Select(x => new SelectListItem(x.BusinessName, x.Id.ToString(), x.Id == tenantId)).ToListAsync();
        var query = db.SaasPayments.IgnoreQueryFilters().Include(x => x.Tenant).Include(x => x.SubscriptionPlan).AsQueryable();
        if (tenantId.HasValue) query = query.Where(x => x.TenantId == tenantId.Value);
        return View(await query.OrderByDescending(x => x.PaymentDate).ToListAsync());
    }

    [HttpGet("Create")]
    public async Task<IActionResult> Create()
    {
        await PopulateListsAsync();
        return View(new SaasPaymentViewModel { InvoiceNumber = $"SAAS-{DateTime.UtcNow:yyyyMMddHHmm}", PaymentDate = DateTime.UtcNow.Date, PaymentStatus = "Paid" });
    }

    [HttpPost("Create"), ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(SaasPaymentViewModel model)
    {
        if (!ModelState.IsValid) { await PopulateListsAsync(); return View(model); }
        db.SaasPayments.Add(new SaasPayment { TenantId = model.TenantId, SubscriptionPlanId = model.SubscriptionPlanId, TenantSubscriptionId = model.TenantSubscriptionId, InvoiceNumber = model.InvoiceNumber, PaymentDate = model.PaymentDate, Amount = model.Amount, PaymentMode = model.PaymentMode, PaymentStatus = model.PaymentStatus, TransactionReference = model.TransactionReference, Notes = model.Notes });
        await db.SaveChangesAsync(); TempData["Success"] = "SaaS payment recorded."; return RedirectToAction(nameof(Index));
    }

    private async Task PopulateListsAsync()
    {
        ViewBag.Tenants = await db.Tenants.IgnoreQueryFilters().Where(x => !x.IsDeleted).Select(x => new SelectListItem(x.BusinessName, x.Id.ToString())).ToListAsync();
        ViewBag.Plans = await db.SubscriptionPlans.Where(x => x.IsActive).Select(x => new SelectListItem(x.PlanName, x.Id.ToString())).ToListAsync();
    }
}
