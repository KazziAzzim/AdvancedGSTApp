using AdvancedGSTApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AdvancedGSTApp.Controllers;

public class AccountController(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager) : Controller
{
    [AllowAnonymous] public IActionResult Login(string? returnUrl = null) => View(new LoginViewModel());
    [HttpPost, AllowAnonymous, ValidateAntiForgeryToken] public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null)
    { if (!ModelState.IsValid) return View(model); var result = await signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, true); if (result.Succeeded) return Redirect(returnUrl ?? Url.Action("Index", "Home")!); ModelState.AddModelError(string.Empty, "Invalid login attempt."); return View(model); }
    [Authorize] public async Task<IActionResult> Logout() { await signInManager.SignOutAsync(); return RedirectToAction(nameof(Login)); }
    [Authorize(Roles="Super Admin,Admin")] public IActionResult CreateUser() => View();
    [Authorize] public IActionResult AccessDenied() => View();
}
