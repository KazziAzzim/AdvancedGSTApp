using AdvancedGSTApp.Models;
using AdvancedGSTApp.Services.Interfaces;

namespace AdvancedGSTApp.Services;

public class GstCalculationService : IGstCalculationService
{
    public GstTaxBreakup Calculate(decimal quantity, decimal rate, decimal discountPercent, decimal gstPercent, string companyState, string partyState)
    {
        var gross = quantity * rate; var discount = gross * discountPercent / 100; var taxable = gross - discount; var gst = taxable * gstPercent / 100;
        var sameState = string.Equals(companyState?.Trim(), partyState?.Trim(), StringComparison.OrdinalIgnoreCase);
        var cgst = sameState ? gst / 2 : 0; var sgst = sameState ? gst / 2 : 0; var igst = sameState ? 0 : gst;
        return new GstTaxBreakup(taxable, cgst, sgst, igst, gst, taxable + gst, sameState ? gstPercent / 2 : 0, sameState ? gstPercent / 2 : 0, sameState ? 0 : gstPercent);
    }
    public void Recalculate(SalesInvoice invoice, string companyState)
    {
        foreach (var item in invoice.Items)
        {
            var tax = Calculate(item.Quantity, item.Rate, item.DiscountPercentage, item.GSTPercentage, companyState, invoice.PlaceOfSupply);
            item.DiscountAmount = item.Quantity * item.Rate * item.DiscountPercentage / 100; item.TaxableAmount = tax.TaxableAmount; item.CGSTPercentage = tax.CGSTRate; item.CGSTAmount = tax.CGSTAmount; item.SGSTPercentage = tax.SGSTRate; item.SGSTAmount = tax.SGSTAmount; item.IGSTPercentage = tax.IGSTRate; item.IGSTAmount = tax.IGSTAmount; item.LineTotal = tax.GrandTotal;
        }
        invoice.TaxableAmount = invoice.Items.Sum(i => i.TaxableAmount); invoice.CGSTAmount = invoice.Items.Sum(i => i.CGSTAmount); invoice.SGSTAmount = invoice.Items.Sum(i => i.SGSTAmount); invoice.IGSTAmount = invoice.Items.Sum(i => i.IGSTAmount); invoice.TotalGSTAmount = invoice.CGSTAmount + invoice.SGSTAmount + invoice.IGSTAmount; invoice.DiscountAmount = invoice.Items.Sum(i => i.DiscountAmount); invoice.GrandTotal = invoice.TaxableAmount + invoice.TotalGSTAmount + invoice.RoundOff; invoice.AmountInWords = $"INR {invoice.GrandTotal:N2}";
    }
}
