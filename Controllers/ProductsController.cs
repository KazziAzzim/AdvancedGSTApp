using AdvancedGSTApp.Filters;
using AdvancedGSTApp.Models;
using AdvancedGSTApp.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AdvancedGSTApp.Controllers;

[Authorize]
[PermissionAuthorize("Products", "View")]
public class ProductsController(IProductService service, IGstApiService gstApi) : Controller
{
    public async Task<IActionResult> Index() => View(await service.GetAllAsync());

    [PermissionAuthorize("Products", "Create")]
    public IActionResult Create() => View("Edit", new Product());

    [PermissionAuthorize("Products", "Edit")]
    public async Task<IActionResult> Edit(int id) => View(await service.GetByIdAsync(id));

    [HttpPost, ValidateAntiForgeryToken]
    [PermissionAuthorize("Products", "Edit")]
    public async Task<IActionResult> Edit(Product model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        await service.SaveAsync(model);
        return RedirectToAction(nameof(Index));
    }

    [HttpPost, ValidateAntiForgeryToken]
    [PermissionAuthorize("Products", "Delete")]
    public async Task<IActionResult> Delete(int id)
    {
        await service.DeleteAsync(id);
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> ValidateHsn(string code) => Json(await gstApi.ValidateHsnSacAsync(code));
}
