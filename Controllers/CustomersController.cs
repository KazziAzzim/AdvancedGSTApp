using AdvancedGSTApp.Models;
using AdvancedGSTApp.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace AdvancedGSTApp.Controllers;
[Authorize]
public class CustomersController(ICustomerService service, IGstApiService gstApi) : Controller
{ public async Task<IActionResult> Index() => View(await service.GetAllAsync()); public async Task<IActionResult> Details(int id) => View(await service.GetByIdAsync(id)); public IActionResult Create() => View("Edit", new Customer()); public async Task<IActionResult> Edit(int id) => View(await service.GetByIdAsync(id)); [HttpPost, ValidateAntiForgeryToken] public async Task<IActionResult> Edit(Customer model) { if (!ModelState.IsValid) return View(model); await service.SaveAsync(model); TempData["Success"] = "Customer saved"; return RedirectToAction(nameof(Index)); } [HttpPost, ValidateAntiForgeryToken] public async Task<IActionResult> Delete(int id) { await service.DeleteAsync(id); return RedirectToAction(nameof(Index)); } public async Task<IActionResult> ValidateGstin(string gstin) => Json(await gstApi.ValidateGstinAsync(gstin)); public async Task<IActionResult> Statement(int id) => View(await service.GetByIdAsync(id)); }
