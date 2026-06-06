using AdvancedGSTApp.Data;
using AdvancedGSTApp.Models;
using AdvancedGSTApp.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AdvancedGSTApp.Services;
public class PurchaseInvoiceService(ApplicationDbContext db) : IPurchaseInvoiceService
{
    public Task<List<PurchaseInvoice>> GetAllAsync() => db.PurchaseInvoices.Include(x => x.Supplier).OrderByDescending(x => x.InvoiceDate).ToListAsync();
    public Task<PurchaseInvoice?> GetByIdAsync(int id) => db.PurchaseInvoices.Include(x => x.Supplier).Include(x => x.Items).FirstOrDefaultAsync(x => x.Id == id);
    public async Task<PurchaseInvoice> SaveAsync(PurchaseInvoice invoice) { invoice.ITCEligibleAmount = invoice.CGSTAmount + invoice.SGSTAmount + invoice.IGSTAmount; if (invoice.Id == 0) db.PurchaseInvoices.Add(invoice); else db.PurchaseInvoices.Update(invoice); db.InputTaxCreditLedgers.Add(new InputTaxCreditLedger { LedgerType="ITC", TransactionDate=invoice.InvoiceDate, ReferenceType="PurchaseInvoice", ReferenceNumber=invoice.SupplierInvoiceNumber, CGSTCredit=invoice.CGSTAmount, SGSTCredit=invoice.SGSTAmount, IGSTCredit=invoice.IGSTAmount, Balance=invoice.ITCEligibleAmount, Remarks="ITC from purchase" }); await db.SaveChangesAsync(); return invoice; }
}
