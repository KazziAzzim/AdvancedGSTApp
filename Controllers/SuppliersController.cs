using AdvancedGSTApp.Filters;
using AdvancedGSTApp.Models;
using AdvancedGSTApp.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AdvancedGSTApp.Controllers;

[Authorize]
[PermissionAuthorize("Suppliers", "View")]
public class SuppliersController(ISupplierService service, IGstApiService gstApi) : Controller
{
    public async Task<IActionResult> Index() => View(await service.GetAllAsync());

    [PermissionAuthorize("Suppliers", "Create")]
    public IActionResult Create() => View("Edit", new Supplier());

    [PermissionAuthorize("Suppliers", "Edit")]
    public async Task<IActionResult> Edit(int id) => View(await service.GetByIdAsync(id));

    [HttpPost, ValidateAntiForgeryToken]
    [PermissionAuthorize("Suppliers", "Edit")]
    public async Task<IActionResult> Edit(Supplier model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        await service.SaveAsync(model);
        return RedirectToAction(nameof(Index));
    }

    [HttpPost, ValidateAntiForgeryToken]
    [PermissionAuthorize("Suppliers", "Delete")]
    public async Task<IActionResult> Delete(int id)
    {
        await service.DeleteAsync(id);
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> ValidateGstin(string gstin) => Json(await gstApi.ValidateGstinAsync(gstin));

    public async Task<IActionResult> Statement(int id) => View(await service.GetByIdAsync(id));
}
