using AdvancedGSTApp.Filters;
using AdvancedGSTApp.Data;
using AdvancedGSTApp.Models;
using AdvancedGSTApp.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace AdvancedGSTApp.Controllers;

[Authorize]
[PermissionAuthorize("Sales Invoice", "View")]
public class SalesInvoicesController(
    ApplicationDbContext db,
    ISalesInvoiceService invoices,
    IEInvoiceService einvoice,
    IEWayBillService eway,
    IPdfService pdf,
    ITenantContext tenantContext,
    ISubscriptionService subscriptionService) : Controller
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

        if (tenantContext.TenantId.HasValue && !await subscriptionService.CanCreateInvoiceAsync(tenantContext.TenantId.Value))
        {
            ModelState.AddModelError(string.Empty, "Your monthly invoice limit has been reached for the current subscription plan.");
        }

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
    public async Task<IActionResult> Print(int id)
    {
        var invoiceQuery = db.SalesInvoices
            .Include(x => x.Customer)
            .Include(x => x.Items)
            .AsQueryable();

        if (tenantContext.TenantId.HasValue)
        {
            invoiceQuery = invoiceQuery.Where(x => x.TenantId == tenantContext.TenantId.Value);
        }

        var invoice = await invoiceQuery.FirstOrDefaultAsync(x => x.Id == id);
        if (invoice is null)
        {
            return NotFound();
        }

        var companyQuery = db.CompanyProfiles.Where(x => x.IsActive);
        var eInvoiceQuery = db.EInvoices.Where(x => x.SalesInvoiceId == id);
        var eWayBillQuery = db.EWayBills.Where(x => x.SalesInvoiceId == id);

        if (tenantContext.TenantId.HasValue)
        {
            companyQuery = companyQuery.Where(x => x.TenantId == tenantContext.TenantId.Value);
            eInvoiceQuery = eInvoiceQuery.Where(x => x.TenantId == tenantContext.TenantId.Value);
            eWayBillQuery = eWayBillQuery.Where(x => x.TenantId == tenantContext.TenantId.Value);
        }
        else
        {
            companyQuery = companyQuery.Where(x => x.TenantId == invoice.TenantId);
            eInvoiceQuery = eInvoiceQuery.Where(x => x.TenantId == invoice.TenantId);
            eWayBillQuery = eWayBillQuery.Where(x => x.TenantId == invoice.TenantId);
        }

        var model = new TaxInvoicePrintViewModel
        {
            Invoice = invoice,
            Company = await companyQuery.OrderByDescending(x => x.UpdatedDate ?? x.CreatedDate).FirstOrDefaultAsync(),
            Customer = invoice.Customer,
            Items = invoice.Items.OrderBy(x => x.Id).ToList(),
            EInvoice = await eInvoiceQuery.OrderByDescending(x => x.CreatedDate).FirstOrDefaultAsync(),
            EWayBill = await eWayBillQuery.OrderByDescending(x => x.CreatedDate).FirstOrDefaultAsync(),
            AmountInWords = string.IsNullOrWhiteSpace(invoice.AmountInWords)
                ? ConvertToIndianCurrencyWords(invoice.GrandTotal)
                : invoice.AmountInWords
        };

        model.GstSummary = model.Items
            .GroupBy(x => x.GSTPercentage)
            .Select(x => new TaxInvoiceGstSummaryRow
            {
                GstRate = x.Key,
                TaxableAmount = x.Sum(i => i.TaxableAmount),
                CgstAmount = x.Sum(i => i.CGSTAmount),
                SgstAmount = x.Sum(i => i.SGSTAmount),
                IgstAmount = x.Sum(i => i.IGSTAmount)
            })
            .OrderBy(x => x.GstRate)
            .ToList();

        return View(model);
    }

    [PermissionAuthorize("Sales Invoice", "Export")]
    public async Task<IActionResult> DownloadPdf(int id) => File(await pdf.GenerateInvoicePdfAsync(id), "application/pdf", $"invoice-{id}.pdf");

    [HttpPost, ValidateAntiForgeryToken]
    [PermissionAuthorize("E-Invoice", "Generate")]
    public async Task<IActionResult> GenerateEInvoice(int id)
    {
        if (tenantContext.TenantId.HasValue && !await subscriptionService.HasFeatureAsync(tenantContext.TenantId.Value, "e-invoice"))
        {
            TempData["Error"] = "Your subscription plan does not include e-invoice.";
            return RedirectToAction(nameof(Details), new { id });
        }
        var result = await einvoice.GenerateAsync(id);
        TempData[result.Success ? "Success" : "Error"] = result.Message;
        return RedirectToAction(nameof(Details), new { id });
    }

    [HttpPost, ValidateAntiForgeryToken]
    [PermissionAuthorize("E-Way Bill", "Generate")]
    public async Task<IActionResult> GenerateEWayBill(int id)
    {
        if (tenantContext.TenantId.HasValue && !await subscriptionService.HasFeatureAsync(tenantContext.TenantId.Value, "e-way bill"))
        {
            TempData["Error"] = "Your subscription plan does not include e-way bill.";
            return RedirectToAction(nameof(Details), new { id });
        }
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

    private static string ConvertToIndianCurrencyWords(decimal amount)
    {
        var rupees = (long)Math.Floor(amount);
        var paise = (int)Math.Round((amount - rupees) * 100, MidpointRounding.AwayFromZero);

        if (paise == 100)
        {
            rupees++;
            paise = 0;
        }

        var words = new StringBuilder($"Rupees {ConvertNumberToWords(rupees)}");
        if (paise > 0)
        {
            words.Append($" and {ConvertNumberToWords(paise)} Paise");
        }

        words.Append(" Only");
        return words.ToString();
    }

    private static string ConvertNumberToWords(long number)
    {
        if (number == 0)
        {
            return "Zero";
        }

        string[] units = ["", "One", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine", "Ten", "Eleven", "Twelve", "Thirteen", "Fourteen", "Fifteen", "Sixteen", "Seventeen", "Eighteen", "Nineteen"];
        string[] tens = ["", "", "Twenty", "Thirty", "Forty", "Fifty", "Sixty", "Seventy", "Eighty", "Ninety"];

        static string TwoDigitWords(long value, string[] unitWords, string[] tenWords)
        {
            if (value < 20)
            {
                return unitWords[(int)value];
            }

            return string.IsNullOrWhiteSpace(unitWords[(int)(value % 10)])
                ? tenWords[(int)(value / 10)]
                : $"{tenWords[(int)(value / 10)]} {unitWords[(int)(value % 10)]}";
        }

        var parts = new List<string>();
        var crore = number / 10000000;
        number %= 10000000;
        var lakh = number / 100000;
        number %= 100000;
        var thousand = number / 1000;
        number %= 1000;
        var hundred = number / 100;
        var remainder = number % 100;

        if (crore > 0) parts.Add($"{ConvertNumberToWords(crore)} Crore");
        if (lakh > 0) parts.Add($"{ConvertNumberToWords(lakh)} Lakh");
        if (thousand > 0) parts.Add($"{ConvertNumberToWords(thousand)} Thousand");
        if (hundred > 0) parts.Add($"{units[(int)hundred]} Hundred");
        if (remainder > 0) parts.Add(TwoDigitWords(remainder, units, tens));

        return string.Join(" ", parts);
    }
}
