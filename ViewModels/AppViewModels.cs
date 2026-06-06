using System.ComponentModel.DataAnnotations;

namespace AdvancedGSTApp.Models;

public class LoginViewModel
{
    [Required, EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required, DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;

    public bool RememberMe { get; set; }
}

public class DashboardViewModel
{
    public decimal TotalSalesAmount { get; set; }

    public decimal TotalPurchaseAmount { get; set; }

    public decimal TotalGstPayable { get; set; }

    public decimal TotalInputTaxCredit { get; set; }

    public int TotalCustomers { get; set; }

    public int TotalSuppliers { get; set; }

    public List<SalesInvoice> RecentSalesInvoices { get; set; } = [];

    public List<PurchaseInvoice> RecentPurchaseInvoices { get; set; } = [];

    public Dictionary<string, decimal> MonthlySalesSummary { get; set; } = [];

    public Dictionary<string, decimal> MonthlyPurchaseSummary { get; set; } = [];

    public int PendingGstReturns { get; set; }

    public Dictionary<string, int> EInvoiceStatusSummary { get; set; } = [];

    public Dictionary<string, int> EWayBillStatusSummary { get; set; } = [];
}

public class GstPayableSummaryViewModel
{
    public decimal OutputCGST { get; set; }

    public decimal OutputSGST { get; set; }

    public decimal OutputIGST { get; set; }

    public decimal InputCGST { get; set; }

    public decimal InputSGST { get; set; }

    public decimal InputIGST { get; set; }

    public decimal NetCGSTPayable => OutputCGST - InputCGST;

    public decimal NetSGSTPayable => OutputSGST - InputSGST;

    public decimal NetIGSTPayable => OutputIGST - InputIGST;

    public decimal TotalGstPayable => NetCGSTPayable + NetSGSTPayable + NetIGSTPayable;
}

public class Gstr1RowViewModel
{
    public string InvoiceNumber { get; set; } = string.Empty;

    public DateTime InvoiceDate { get; set; }

    public string? CustomerGSTIN { get; set; }

    public string CustomerName { get; set; } = string.Empty;

    public string PlaceOfSupply { get; set; } = string.Empty;

    public decimal TaxableAmount { get; set; }

    public decimal GSTRate { get; set; }

    public decimal CGST { get; set; }

    public decimal SGST { get; set; }

    public decimal IGST { get; set; }

    public decimal InvoiceTotal { get; set; }

    public string EInvoiceStatus { get; set; } = string.Empty;

    public string? IRNNumber { get; set; }
}

public class Gstr3BViewModel
{
    public decimal OutwardTaxableSupplies { get; set; }

    public decimal OutputCGST { get; set; }

    public decimal OutputSGST { get; set; }

    public decimal OutputIGST { get; set; }

    public decimal EligibleITC { get; set; }

    public decimal InputCGST { get; set; }

    public decimal InputSGST { get; set; }

    public decimal InputIGST { get; set; }

    public decimal NetTaxPayable { get; set; }
}

public class SalesInvoiceFormViewModel
{
    public SalesInvoice Invoice { get; set; } = new();

    public List<Customer> Customers { get; set; } = [];

    public List<Product> Products { get; set; } = [];

    public List<ServiceItem> Services { get; set; } = [];

    public string CompanyState { get; set; } = string.Empty;
}
