using AdvancedGSTApp.Data;
using AdvancedGSTApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AdvancedGSTApp.Controllers;

[Authorize(Roles = "Super Admin,Support Admin")]
[Route("SuperAdmin/SubscriptionPlans")]
public class SubscriptionPlansController(ApplicationDbContext db) : Controller
{
    [HttpGet("")]
    public async Task<IActionResult> Index() => View(await db.SubscriptionPlans.OrderBy(x => x.MonthlyPrice).ToListAsync());
    [HttpGet("Create")]
    public IActionResult Create() => View(new CreateSubscriptionPlanViewModel());
    [HttpPost("Create"), ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateSubscriptionPlanViewModel model)
    {
        if (!ModelState.IsValid) return View(model);
        db.SubscriptionPlans.Add(ToEntity(model)); await db.SaveChangesAsync(); TempData["Success"] = "Plan created."; return RedirectToAction(nameof(Index));
    }
    [HttpGet("Edit/{id:int}")]
    public async Task<IActionResult> Edit(int id) { var plan = await db.SubscriptionPlans.FindAsync(id); return plan is null ? NotFound() : View(ToViewModel(plan)); }
    [HttpPost("Edit/{id:int}"), ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, EditSubscriptionPlanViewModel model)
    {
        if (id != model.Id) return BadRequest(); if (!ModelState.IsValid) return View(model); var plan = await db.SubscriptionPlans.FindAsync(id); if (plan is null) return NotFound(); Update(plan, model); await db.SaveChangesAsync(); TempData["Success"] = "Plan updated."; return RedirectToAction(nameof(Index));
    }
    [HttpPost("Toggle/{id:int}"), ValidateAntiForgeryToken]
    public async Task<IActionResult> Toggle(int id) { var plan = await db.SubscriptionPlans.FindAsync(id); if (plan is not null) { plan.IsActive = !plan.IsActive; plan.UpdatedDate = DateTime.UtcNow; await db.SaveChangesAsync(); } return RedirectToAction(nameof(Index)); }
    private static SubscriptionPlan ToEntity(SubscriptionPlanViewModel m) { var p = new SubscriptionPlan(); Update(p, m); return p; }
    private static EditSubscriptionPlanViewModel ToViewModel(SubscriptionPlan p) => new() { Id = p.Id, PlanName = p.PlanName, Description = p.Description, MonthlyPrice = p.MonthlyPrice, YearlyPrice = p.YearlyPrice, MaxUsers = p.MaxUsers, MaxInvoicesPerMonth = p.MaxInvoicesPerMonth, MaxCompanies = p.MaxCompanies, HasEInvoice = p.HasEInvoice, HasEWayBill = p.HasEWayBill, HasGstReconciliation = p.HasGstReconciliation, HasGstApiIntegration = p.HasGstApiIntegration, HasAdvancedReports = p.HasAdvancedReports, IsActive = p.IsActive };
    private static void Update(SubscriptionPlan p, SubscriptionPlanViewModel m) { p.PlanName = m.PlanName; p.Description = m.Description; p.MonthlyPrice = m.MonthlyPrice; p.YearlyPrice = m.YearlyPrice; p.MaxUsers = m.MaxUsers; p.MaxInvoicesPerMonth = m.MaxInvoicesPerMonth; p.MaxCompanies = m.MaxCompanies; p.HasEInvoice = m.HasEInvoice; p.HasEWayBill = m.HasEWayBill; p.HasGstReconciliation = m.HasGstReconciliation; p.HasGstApiIntegration = m.HasGstApiIntegration; p.HasAdvancedReports = m.HasAdvancedReports; p.IsActive = m.IsActive; p.UpdatedDate = DateTime.UtcNow; }
}
