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

public class TenantListViewModel
{
    public List<Tenant> Tenants { get; set; } = [];
    public string? Search { get; set; }
}

public class CreateTenantViewModel
{
    [Required, StringLength(200)] public string BusinessName { get; set; } = string.Empty;
    //[Required, RegularExpression(@"^[0-9]{2}[A-Z]{5}[0-9]{4}[A-Z][1-9A-Z]Z[0-9A-Z]$")]
    public string GSTIN { get; set; } = string.Empty;
    [Required, RegularExpression(@"^[A-Z]{5}[0-9]{4}[A-Z]$")] public string PANNumber { get; set; } = string.Empty;
    [Required, StringLength(150)] public string ContactPersonName { get; set; } = string.Empty;
    [Required, EmailAddress] public string Email { get; set; } = string.Empty;
    [Phone] public string PhoneNumber { get; set; } = string.Empty;
    [Required] public string Address { get; set; } = string.Empty;
    [Required] public string State { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string Pincode { get; set; } = string.Empty;
    public string? Subdomain { get; set; }
    [Required, DataType(DataType.Password), StringLength(100, MinimumLength = 8)] public string Password { get; set; } = "Admin@123";
    [Required, DataType(DataType.Password), Compare(nameof(Password))] public string ConfirmPassword { get; set; } = "Admin@123";
}

public class EditTenantViewModel : CreateTenantViewModel
{
    public int Id { get; set; }
    public bool IsActive { get; set; }
    public bool IsTrial { get; set; }
    public int? SubscriptionPlanId { get; set; }
    public DateTime? SubscriptionEndDate { get; set; }
}

public class TenantDetailsViewModel
{
    public Tenant Tenant { get; set; } = new();
    public TenantSubscription? CurrentSubscription { get; set; }
    public List<SaasPayment> Payments { get; set; } = [];
}

public class TenantRegistrationViewModel : CreateTenantViewModel { }

public class SubscriptionPlanViewModel
{
    public int Id { get; set; }
    [Required] public string PlanName { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal MonthlyPrice { get; set; }
    public decimal YearlyPrice { get; set; }
    public int MaxUsers { get; set; }
    public int MaxInvoicesPerMonth { get; set; }
    public int MaxCompanies { get; set; } = 1;
    public bool HasEInvoice { get; set; }
    public bool HasEWayBill { get; set; }
    public bool HasGstReconciliation { get; set; }
    public bool HasGstApiIntegration { get; set; }
    public bool HasAdvancedReports { get; set; }
    public bool IsActive { get; set; } = true;
}
public class CreateSubscriptionPlanViewModel : SubscriptionPlanViewModel { }
public class EditSubscriptionPlanViewModel : SubscriptionPlanViewModel { }

public class TenantSubscriptionViewModel
{
    public int Id { get; set; }
    public int TenantId { get; set; }
    public int SubscriptionPlanId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string BillingCycle { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public string Status { get; set; } = "Active";
    public string PaymentStatus { get; set; } = "Pending";
    public string? PaymentReferenceNumber { get; set; }
}

public class SaasPaymentViewModel
{
    public int Id { get; set; }
    public int TenantId { get; set; }
    public int SubscriptionPlanId { get; set; }
    public int? TenantSubscriptionId { get; set; }
    [Required] public string InvoiceNumber { get; set; } = string.Empty;
    public DateTime PaymentDate { get; set; } = DateTime.UtcNow;
    public decimal Amount { get; set; }
    public string PaymentMode { get; set; } = "Manual";
    public string PaymentStatus { get; set; } = "Paid";
    public string? TransactionReference { get; set; }
    public string? Notes { get; set; }
}

public class TenantUsageViewModel
{
    public int TenantId { get; set; }
    public string BusinessName { get; set; } = string.Empty;
    public int UsersUsed { get; set; }
    public int MaxUsers { get; set; }
    public int InvoicesThisMonth { get; set; }
    public int MaxInvoicesPerMonth { get; set; }
}

public class PlatformDashboardViewModel
{
    public int TotalTenants { get; set; }
    public int ActiveTenants { get; set; }
    public int TrialTenants { get; set; }
    public int ExpiredTenants { get; set; }
    public decimal MonthlyRecurringRevenue { get; set; }
    public decimal TotalPaymentsReceived { get; set; }
    public List<Tenant> RecentTenantRegistrations { get; set; } = [];
    public List<TenantSubscription> ExpiringSubscriptions { get; set; } = [];
    public List<TenantUsageViewModel> TenantUsageSummary { get; set; } = [];
}

public class TenantDashboardViewModel
{
    public decimal TotalSales { get; set; }
    public decimal TotalPurchases { get; set; }
    public decimal GstPayable { get; set; }
    public decimal Itc { get; set; }
    public int Customers { get; set; }
    public int Suppliers { get; set; }
    public int Users { get; set; }
    public string CurrentPlan { get; set; } = string.Empty;
    public DateTime? SubscriptionExpiryDate { get; set; }
    public int InvoiceUsageThisMonth { get; set; }
    public int MaxInvoicesPerMonth { get; set; }
    public int PendingGstReturns { get; set; }
    public List<SalesInvoice> RecentInvoices { get; set; } = [];
}
