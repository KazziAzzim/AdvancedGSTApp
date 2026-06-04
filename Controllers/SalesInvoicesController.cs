using AdvancedGSTApp.Filters;
using AdvancedGSTApp.Data;
using AdvancedGSTApp.Models;
using AdvancedGSTApp.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AdvancedGSTApp.Controllers;

[Authorize]
[PermissionAuthorize("Sales Invoice", "View")]
public class SalesInvoicesController(
    ApplicationDbContext db,
    ISalesInvoiceService invoices,
    IEInvoiceService einvoice,
    IEWayBillService eway,
    IPdfService pdf) : Controller
{
    public async Task<IActionResult> Index() => View(await invoices.GetAllAsync());

    [PermissionAuthorize("Sales Invoice", "Create")]
    public async Task<IActionResult> Create() => View("Edit", await BuildFormViewModelAsync(new SalesInvoice
    {
        InvoiceNumber = await invoices.GenerateNextInvoiceNumberAsync(),
        InvoiceDate = DateTime.Today,
        Items = [new SalesInvoiceItem { Quantity = 1 }]
    }));

    [HttpPost, ValidateAntiForgeryToken]
    [PermissionAuthorize("Sales Invoice", "Create")]
    public async Task<IActionResult> Create(SalesInvoiceFormViewModel model)
    {
        var invoice = model.Invoice;

        if (invoice.Items.Count == 0)
        {
            ModelState.AddModelError("Items", "Add at least one invoice item.");
        }

        if (!ModelState.IsValid)
        {
            return View("Edit", await BuildFormViewModelAsync(invoice));
        }

        await invoices.CreateAsync(invoice);
        TempData["Success"] = "Sales invoice saved";
        return RedirectToAction(nameof(Details), new { invoice.Id });
    }

    public async Task<IActionResult> Details(int id) => View(await invoices.GetByIdAsync(id));

    [PermissionAuthorize("Sales Invoice", "Print")]
    public async Task<IActionResult> Print(int id) => View("Details", await invoices.GetByIdAsync(id));

    [PermissionAuthorize("Sales Invoice", "Export")]
    public async Task<IActionResult> DownloadPdf(int id) => File(await pdf.GenerateInvoicePdfAsync(id), "application/pdf", $"invoice-{id}.pdf");

    [HttpPost, ValidateAntiForgeryToken]
    [PermissionAuthorize("E-Invoice", "Generate")]
    public async Task<IActionResult> GenerateEInvoice(int id)
    {
        var result = await einvoice.GenerateAsync(id);
        TempData[result.Success ? "Success" : "Error"] = result.Message;
        return RedirectToAction(nameof(Details), new { id });
    }

    [HttpPost, ValidateAntiForgeryToken]
    [PermissionAuthorize("E-Way Bill", "Generate")]
    public async Task<IActionResult> GenerateEWayBill(int id)
    {
        var result = await eway.GenerateAsync(id);
        TempData[result.Success ? "Success" : "Error"] = result.Message;
        return RedirectToAction(nameof(Details), new { id });
    }

    [HttpPost, ValidateAntiForgeryToken]
    [PermissionAuthorize("Sales Invoice", "Delete")]
    public async Task<IActionResult> Delete(int id)
    {
        await invoices.DeleteAsync(id);
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> ItemLookup(string type, int id)
    {
        if (type == "Service")
        {
            var service = await db.ServiceItems.FindAsync(id);
            return Json(new
            {
                description = service?.ServiceName,
                hsnSac = service?.SACCode,
                gstPercentage = service?.GSTPercentage,
                rate = service?.Rate,
                unit = "Service"
            });
        }

        var product = await db.Products.FindAsync(id);
        return Json(new
        {
            description = product?.ProductName,
            hsnSac = product?.HSNCode,
            gstPercentage = product?.GSTPercentage,
            rate = product?.SaleRate,
            unit = product?.Unit
        });
    }

    private async Task<SalesInvoiceFormViewModel> BuildFormViewModelAsync(SalesInvoice invoice) => new()
    {
        Invoice = invoice,
        Customers = await db.Customers.ToListAsync(),
        Products = await db.Products.ToListAsync(),
        Services = await db.ServiceItems.ToListAsync(),
        CompanyState = await db.CompanyProfiles.Select(x => x.State).FirstOrDefaultAsync() ?? string.Empty
    };
}
