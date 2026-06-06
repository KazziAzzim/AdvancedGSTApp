using AdvancedGSTApp.Models;
using AdvancedGSTApp.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AdvancedGSTApp.Controllers;

[AllowAnonymous]
[Route("Account/RegisterTenant")]
public class TenantRegistrationController(ITenantService tenantService) : Controller
{
    [HttpGet]
    public IActionResult Index() => View(new TenantRegistrationViewModel());

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Index(TenantRegistrationViewModel model)
    {
        if (!ModelState.IsValid) return View(model);
        var tenant = new Tenant
        {
            TenantName = model.BusinessName.Trim().ToLowerInvariant().Replace(' ', '-'),
            BusinessName = model.BusinessName.Trim(),
            GSTIN = model.GSTIN.Trim().ToUpperInvariant(),
            PANNumber = model.PANNumber.Trim().ToUpperInvariant(),
            ContactPersonName = model.ContactPersonName.Trim(),
            Email = model.Email.Trim(),
            PhoneNumber = model.PhoneNumber,
            Address = model.Address,
            State = model.State,
            City = model.City,
            Pincode = model.Pincode,
            Subdomain = model.Subdomain
        };
        await tenantService.CreateTenantAsync(tenant, model.Email.Trim(), model.Password);
        TempData["Success"] = "Tenant registration completed. Please sign in with your tenant admin account.";
        return RedirectToAction("Login", "Account");
    }
}
