using AdvancedGSTApp.Data;
using AdvancedGSTApp.Models;
using AdvancedGSTApp.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AdvancedGSTApp.Controllers;
[Authorize]
public class SalesInvoicesController(ApplicationDbContext db, ISalesInvoiceService invoices, IEInvoiceService einvoice, IEWayBillService eway, IPdfService pdf) : Controller
{
    public async Task<IActionResult> Index() => View(await invoices.GetAllAsync());
    public async Task<IActionResult> Create() => View("Edit", new SalesInvoiceFormViewModel { Invoice = new SalesInvoice { InvoiceNumber = await invoices.GenerateNextInvoiceNumberAsync(), InvoiceDate = DateTime.Today, Items = [new SalesInvoiceItem { Quantity = 1 }] }, Customers = await db.Customers.ToListAsync(), Products = await db.Products.ToListAsync(), Services = await db.ServiceItems.ToListAsync(), CompanyState = await db.CompanyProfiles.Select(x => x.State).FirstOrDefaultAsync() ?? string.Empty });
    [HttpPost, ValidateAntiForgeryToken] public async Task<IActionResult> Create(SalesInvoiceFormViewModel model) { var invoice = model.Invoice; if (invoice.Items.Count == 0) ModelState.AddModelError("Items", "Add at least one invoice item."); if (!ModelState.IsValid) return View("Edit", new SalesInvoiceFormViewModel { Invoice = invoice, Customers = await db.Customers.ToListAsync(), Products = await db.Products.ToListAsync(), Services = await db.ServiceItems.ToListAsync(), CompanyState = await db.CompanyProfiles.Select(x => x.State).FirstOrDefaultAsync() ?? string.Empty }); await invoices.CreateAsync(invoice); TempData["Success"] = "Sales invoice saved"; return RedirectToAction(nameof(Details), new { invoice.Id }); }
    public async Task<IActionResult> Details(int id) => View(await invoices.GetByIdAsync(id));
    public async Task<IActionResult> Print(int id) => View("Details", await invoices.GetByIdAsync(id));
    public async Task<IActionResult> DownloadPdf(int id) => File(await pdf.GenerateInvoicePdfAsync(id), "application/pdf", $"invoice-{id}.pdf");
    [HttpPost, ValidateAntiForgeryToken] public async Task<IActionResult> GenerateEInvoice(int id) { var r = await einvoice.GenerateAsync(id); TempData[r.Success ? "Success" : "Error"] = r.Message; return RedirectToAction(nameof(Details), new { id }); }
    [HttpPost, ValidateAntiForgeryToken] public async Task<IActionResult> GenerateEWayBill(int id) { var r = await eway.GenerateAsync(id); TempData[r.Success ? "Success" : "Error"] = r.Message; return RedirectToAction(nameof(Details), new { id }); }
    [HttpPost, ValidateAntiForgeryToken] public async Task<IActionResult> Delete(int id) { await invoices.DeleteAsync(id); return RedirectToAction(nameof(Index)); }
    public async Task<IActionResult> ItemLookup(string type, int id) { if (type == "Service") { var s = await db.ServiceItems.FindAsync(id); return Json(new { description = s?.ServiceName, hsnSac = s?.SACCode, gstPercentage = s?.GSTPercentage, rate = s?.Rate, unit = "Service" }); } var p = await db.Products.FindAsync(id); return Json(new { description = p?.ProductName, hsnSac = p?.HSNCode, gstPercentage = p?.GSTPercentage, rate = p?.SaleRate, unit = p?.Unit }); }
}
