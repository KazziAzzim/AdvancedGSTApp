using AdvancedGSTApp.Data;
using AdvancedGSTApp.Models;
using AdvancedGSTApp.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AdvancedGSTApp.Services;

public class SalesInvoiceService(ApplicationDbContext db, IGstCalculationService calc) : ISalesInvoiceService
{
    public Task<List<SalesInvoice>> GetAllAsync() => db.SalesInvoices.Include(x => x.Customer).OrderByDescending(x => x.InvoiceDate).ToListAsync();
    public Task<SalesInvoice?> GetByIdAsync(int id) => db.SalesInvoices.Include(x => x.Customer).Include(x => x.Items).FirstOrDefaultAsync(x => x.Id == id);
    public async Task<string> GenerateNextInvoiceNumberAsync() { var series = await db.InvoiceSeries.FirstAsync(x => x.IsActive); return series.NextNumber(); }
    public async Task<SalesInvoice> CreateAsync(SalesInvoice invoice)
    {
        var companyState = await db.CompanyProfiles.Where(x => x.IsActive).Select(x => x.State).FirstOrDefaultAsync() ?? invoice.PlaceOfSupply;
        if (string.IsNullOrWhiteSpace(invoice.InvoiceNumber)) invoice.InvoiceNumber = await GenerateNextInvoiceNumberAsync();
        calc.Recalculate(invoice, companyState); db.SalesInvoices.Add(invoice);
        var series = await db.InvoiceSeries.FirstAsync(x => x.IsActive); series.CurrentNumber++;
        db.OutputTaxLedgers.Add(new OutputTaxLedger { LedgerType = "Output", TransactionDate = invoice.InvoiceDate, ReferenceType = "SalesInvoice", ReferenceNumber = invoice.InvoiceNumber, CGSTCredit = invoice.CGSTAmount, SGSTCredit = invoice.SGSTAmount, IGSTCredit = invoice.IGSTAmount, Balance = invoice.TotalGSTAmount, Remarks = "Output GST from sales" });
        await db.SaveChangesAsync(); return invoice;
    }
    public async Task DeleteAsync(int id) { var invoice = await db.SalesInvoices.FindAsync(id); if (invoice != null) { invoice.IsDeleted = true; await db.SaveChangesAsync(); } }
}
