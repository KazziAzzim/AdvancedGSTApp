using AdvancedGSTApp.Data;
using AdvancedGSTApp.Models;
using AdvancedGSTApp.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AdvancedGSTApp.Controllers;

[Authorize(Roles = "Super Admin,Support Admin")]
[Route("SuperAdmin/Tenants")]
public class TenantsController(ApplicationDbContext db, ITenantService tenantService) : Controller
{
    [HttpGet("")]
    public async Task<IActionResult> Index(string? search)
    {
        var query = db.Tenants.IgnoreQueryFilters().Include(x => x.SubscriptionPlan).Where(x => !x.IsDeleted);
        if (!string.IsNullOrWhiteSpace(search)) query = query.Where(x => x.BusinessName.Contains(search) || x.Email.Contains(search) || x.GSTIN.Contains(search));
        return View(new TenantListViewModel { Search = search, Tenants = await query.OrderByDescending(x => x.CreatedDate).ToListAsync() });
    }

    [HttpGet("Create")]
    public IActionResult Create() => View(new CreateTenantViewModel());

    [HttpPost("Create"), ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateTenantViewModel model)
    {
        if (!ModelState.IsValid) return View(model);
        await tenantService.CreateTenantAsync(new Tenant
        {
            TenantName = model.BusinessName.Trim().ToLowerInvariant().Replace(' ', '-'),
            BusinessName = model.BusinessName.Trim(), GSTIN = model.GSTIN.Trim().ToUpperInvariant(), PANNumber = model.PANNumber.Trim().ToUpperInvariant(),
            ContactPersonName = model.ContactPersonName.Trim(), Email = model.Email.Trim(), PhoneNumber = model.PhoneNumber, Address = model.Address, State = model.State, City = model.City, Pincode = model.Pincode, Subdomain = model.Subdomain
        }, model.Email.Trim(), model.Password);
        TempData["Success"] = "Tenant created with tenant admin user and trial defaults.";
        return RedirectToAction(nameof(Index));
    }

    [HttpGet("Edit/{id:int}")]
    public async Task<IActionResult> Edit(int id)
    {
        var tenant = await db.Tenants.IgnoreQueryFilters().FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
        if (tenant is null) return NotFound();
        return View(new EditTenantViewModel { Id = tenant.Id, BusinessName = tenant.BusinessName, GSTIN = tenant.GSTIN, PANNumber = tenant.PANNumber, ContactPersonName = tenant.ContactPersonName, Email = tenant.Email, PhoneNumber = tenant.PhoneNumber, Address = tenant.Address, State = tenant.State, City = tenant.City, Pincode = tenant.Pincode, Subdomain = tenant.Subdomain, IsActive = tenant.IsActive, IsTrial = tenant.IsTrial, SubscriptionPlanId = tenant.SubscriptionPlanId, SubscriptionEndDate = tenant.SubscriptionEndDate });
    }

    [HttpPost("Edit/{id:int}"), ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, EditTenantViewModel model)
    {
        if (id != model.Id) return BadRequest();
        if (!ModelState.IsValid) return View(model);
        var tenant = await db.Tenants.IgnoreQueryFilters().FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
        if (tenant is null) return NotFound();
        tenant.BusinessName = model.BusinessName; tenant.TenantName = model.BusinessName.Trim().ToLowerInvariant().Replace(' ', '-'); tenant.GSTIN = model.GSTIN; tenant.PANNumber = model.PANNumber; tenant.ContactPersonName = model.ContactPersonName; tenant.Email = model.Email; tenant.PhoneNumber = model.PhoneNumber; tenant.Address = model.Address; tenant.State = model.State; tenant.City = model.City; tenant.Pincode = model.Pincode; tenant.Subdomain = model.Subdomain; tenant.IsActive = model.IsActive; tenant.IsTrial = model.IsTrial; tenant.SubscriptionEndDate = model.SubscriptionEndDate; tenant.UpdatedDate = DateTime.UtcNow;
        await db.SaveChangesAsync();
        TempData["Success"] = "Tenant updated.";
        return RedirectToAction(nameof(Index));
    }

    [HttpGet("Details/{id:int}")]
    public async Task<IActionResult> Details(int id)
    {
        var tenant = await db.Tenants.IgnoreQueryFilters().Include(x => x.SubscriptionPlan).FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
        if (tenant is null) return NotFound();
        var subscription = await db.TenantSubscriptions.IgnoreQueryFilters().Include(x => x.SubscriptionPlan).Where(x => x.TenantId == id).OrderByDescending(x => x.EndDate).FirstOrDefaultAsync();
        var payments = await db.SaasPayments.IgnoreQueryFilters().Where(x => x.TenantId == id).OrderByDescending(x => x.PaymentDate).ToListAsync();
        return View(new TenantDetailsViewModel { Tenant = tenant, CurrentSubscription = subscription, Payments = payments });
    }

    [HttpPost("Activate/{id:int}"), ValidateAntiForgeryToken]
    public async Task<IActionResult> Activate(int id) { await tenantService.ActivateAsync(id); TempData["Success"] = "Tenant activated."; return RedirectToAction(nameof(Index)); }

    [HttpPost("Deactivate/{id:int}"), ValidateAntiForgeryToken]
    public async Task<IActionResult> Deactivate(int id) { await tenantService.DeactivateAsync(id); TempData["Success"] = "Tenant deactivated."; return RedirectToAction(nameof(Index)); }

    [HttpPost("Delete/{id:int}"), ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var tenant = await db.Tenants.IgnoreQueryFilters().FirstOrDefaultAsync(x => x.Id == id);
        if (tenant is not null) { tenant.IsDeleted = true; tenant.UpdatedDate = DateTime.UtcNow; await db.SaveChangesAsync(); TempData["Success"] = "Tenant soft deleted."; }
        return RedirectToAction(nameof(Index));
    }
}
