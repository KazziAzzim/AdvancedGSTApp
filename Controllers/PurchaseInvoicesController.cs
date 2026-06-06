using AdvancedGSTApp.Filters;
using AdvancedGSTApp.Models;
using AdvancedGSTApp.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AdvancedGSTApp.Controllers;

[Authorize]
[PermissionAuthorize("Purchase Invoice", "View")]
public class PurchaseInvoicesController(IPurchaseInvoiceService service) : Controller
{
    public async Task<IActionResult> Index() => View(await service.GetAllAsync());

    [PermissionAuthorize("Purchase Invoice", "Create")]
    public IActionResult Create() => View("Edit", new PurchaseInvoice());

    [HttpPost, ValidateAntiForgeryToken]
    [PermissionAuthorize("Purchase Invoice", "Create")]
    public async Task<IActionResult> Create(PurchaseInvoice invoice)
    {
        if (!ModelState.IsValid)
        {
            return View("Edit", invoice);
        }

        await service.SaveAsync(invoice);
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Details(int id) => View(await service.GetByIdAsync(id));
}
