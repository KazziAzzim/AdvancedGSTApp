using AdvancedGSTApp.Data;
using AdvancedGSTApp.Models;
using AdvancedGSTApp.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AdvancedGSTApp.Controllers;

public class AccountController(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, ApplicationDbContext db, ISubscriptionService subscriptionService) : Controller
{
    [AllowAnonymous]
    public IActionResult Login(string? returnUrl = null) => View(new LoginViewModel());

    [HttpPost, AllowAnonymous, ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var user = await userManager.FindByEmailAsync(model.Email);
        if (user is not null && !user.IsActive)
        {
            ModelState.AddModelError(string.Empty, "Your account is inactive. Please contact administrator.");
            return View(model);
        }

        if (user is not null && !user.IsPlatformUser && user.TenantId.HasValue)
        {
            var tenant = await db.Tenants.IgnoreQueryFilters().FirstOrDefaultAsync(x => x.Id == user.TenantId.Value);
            if (tenant is null || tenant.IsDeleted || !tenant.IsActive)
            {
                ModelState.AddModelError(string.Empty, "Your tenant is inactive. Please contact support.");
                return View(model);
            }

            if (!await subscriptionService.IsSubscriptionActiveAsync(user.TenantId.Value))
            {
                ModelState.AddModelError(string.Empty, "Your subscription has expired. Please contact support to renew your plan.");
                return View(model);
            }
        }

        if (user is not null)
        {
            var result = await signInManager.CheckPasswordSignInAsync(user, model.Password, true);

            if (result.Succeeded)
            {
                await signInManager.SignInAsync(user, model.RememberMe);
                user.LastLoginDate = DateTime.UtcNow;
                await userManager.UpdateAsync(user);
                return Redirect(returnUrl ?? (user.IsPlatformUser ? Url.Action("Index", "SuperAdminDashboard")! : Url.Action("Index", "TenantDashboard")!));
            }
        }

        ModelState.AddModelError(string.Empty, "Invalid login attempt.");
        return View(model);
    }

    [Authorize]
    public async Task<IActionResult> Logout()
    {
        await signInManager.SignOutAsync();
        return RedirectToAction(nameof(Login));
    }

    [Authorize]
    public IActionResult AccessDenied() => View();
}
