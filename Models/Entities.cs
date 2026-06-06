using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace AdvancedGSTApp.Models;

public class ApplicationUser : IdentityUser
{
    public int? TenantId { get; set; }
    public Tenant? Tenant { get; set; }
    [StringLength(150)] public string FullName { get; set; } = string.Empty;
    [StringLength(300)] public string? ProfilePhoto { get; set; }
    public bool IsActive { get; set; } = true;
    public bool IsPlatformUser { get; set; }
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedDate { get; set; }
    public DateTime? LastLoginDate { get; set; }
}

public class ApplicationRole : IdentityRole
{
    public int? TenantId { get; set; }
    public Tenant? Tenant { get; set; }
    public bool IsPlatformRole { get; set; }
    [StringLength(500)] public string? Description { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedDate { get; set; }
}

public class RolePermission
{
    public int Id { get; set; }
    public int? TenantId { get; set; }
    public Tenant? Tenant { get; set; }
    [Required] public string RoleId { get; set; } = string.Empty;
    public ApplicationRole? Role { get; set; }
    [Required, StringLength(120)] public string ModuleName { get; set; } = string.Empty;
    public bool CanView { get; set; }
    public bool CanCreate { get; set; }
    public bool CanEdit { get; set; }
    public bool CanDelete { get; set; }
    public bool CanPrint { get; set; }
    public bool CanExport { get; set; }
    public bool CanEmail { get; set; }
    public bool CanGenerate { get; set; }
    public bool CanApprove { get; set; }
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedDate { get; set; }
}


public interface ITenantEntity
{
    int TenantId { get; set; }
}

public abstract class TenantEntity : ITenantEntity
{
    public int Id { get; set; }
    public int TenantId { get; set; }
    public Tenant? Tenant { get; set; }
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedDate { get; set; }
    public bool IsDeleted { get; set; }
}

public abstract class AuditableEntity : TenantEntity
{
    public string? CreatedBy { get; set; }
    public string? UpdatedBy { get; set; }
    public string? DeletedBy { get; set; }
    public DateTime? DeletedDate { get; set; }
}

public class Tenant
{
    public int Id { get; set; }
    [Required, StringLength(150)] public string TenantName { get; set; } = string.Empty;
    [Required, StringLength(200)] public string BusinessName { get; set; } = string.Empty;
    [RegularExpression(@"^[0-9]{2}[A-Z]{5}[0-9]{4}[A-Z][1-9A-Z]Z[0-9A-Z]$")] public string GSTIN { get; set; } = string.Empty;
    [RegularExpression(@"^[A-Z]{5}[0-9]{4}[A-Z]$")] public string PANNumber { get; set; } = string.Empty;
    [Required, StringLength(150)] public string ContactPersonName { get; set; } = string.Empty;
    [Required, EmailAddress] public string Email { get; set; } = string.Empty;
    [Phone] public string PhoneNumber { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string State { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string Pincode { get; set; } = string.Empty;
    public string? Subdomain { get; set; }
    public string? DatabaseName { get; set; }
    public string? LogoPath { get; set; }
    public bool IsActive { get; set; } = true;
    public bool IsTrial { get; set; } = true;
    public DateTime? TrialStartDate { get; set; }
    public DateTime? TrialEndDate { get; set; }
    public int? SubscriptionPlanId { get; set; }
    public SubscriptionPlan? SubscriptionPlan { get; set; }
    public DateTime? SubscriptionStartDate { get; set; }
    public DateTime? SubscriptionEndDate { get; set; }
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedDate { get; set; }
    public string? CreatedBy { get; set; }
    public string? UpdatedBy { get; set; }
    public bool IsDeleted { get; set; }
}

public class SubscriptionPlan
{
    public int Id { get; set; }
    [Required, StringLength(100)] public string PlanName { get; set; } = string.Empty;
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
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedDate { get; set; }
}

public class TenantSubscription
{
    public int Id { get; set; }
    public int TenantId { get; set; }
    public Tenant? Tenant { get; set; }
    public int SubscriptionPlanId { get; set; }
    public SubscriptionPlan? SubscriptionPlan { get; set; }
    public DateTime StartDate { get; set; } = DateTime.UtcNow;
    public DateTime EndDate { get; set; } = DateTime.UtcNow.AddDays(14);
    public string BillingCycle { get; set; } = "Trial";
    public decimal Amount { get; set; }
    public string Status { get; set; } = "Trial";
    public string PaymentStatus { get; set; } = "Pending";
    public string? PaymentReferenceNumber { get; set; }
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedDate { get; set; }
}

public class SaasPayment
{
    public int Id { get; set; }
    public int TenantId { get; set; }
    public Tenant? Tenant { get; set; }
    public int SubscriptionPlanId { get; set; }
    public SubscriptionPlan? SubscriptionPlan { get; set; }
    public int? TenantSubscriptionId { get; set; }
    public TenantSubscription? TenantSubscription { get; set; }
    public string InvoiceNumber { get; set; } = string.Empty;
    public DateTime PaymentDate { get; set; } = DateTime.UtcNow;
    public decimal Amount { get; set; }
    public string PaymentMode { get; set; } = "Manual";
    public string PaymentStatus { get; set; } = "Pending";
    public string? TransactionReference { get; set; }
    public string? Notes { get; set; }
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
}

public class CompanyProfile : AuditableEntity
{
    [Required, StringLength(200)] public string CompanyName { get; set; } = string.Empty;
    [RegularExpression(@"^[0-9]{2}[A-Z]{5}[0-9]{4}[A-Z][1-9A-Z]Z[0-9A-Z]$")] public string GSTIN { get; set; } = string.Empty;
    [RegularExpression(@"^[A-Z]{5}[0-9]{4}[A-Z]$")] public string PANNumber { get; set; } = string.Empty;
    [Required] public string Address { get; set; } = string.Empty;
    [Required] public string State { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty; public string Pincode { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty; [EmailAddress] public string Email { get; set; } = string.Empty;
    public string? LogoPath { get; set; }
    public string BankName { get; set; } = string.Empty; public string AccountNumber { get; set; } = string.Empty;
    public string IFSCCode { get; set; } = string.Empty; public string BranchName { get; set; } = string.Empty;
    public string TermsAndConditions { get; set; } = string.Empty; public string DefaultInvoicePrefix { get; set; } = "GST";
    public string FinancialYear { get; set; } = "2026-27"; public bool IsActive { get; set; } = true;
}

public class Customer : AuditableEntity
{
    [Required, StringLength(200)] public string CustomerName { get; set; } = string.Empty;
    [RegularExpression(@"^$|^[0-9]{2}[A-Z]{5}[0-9]{4}[A-Z][1-9A-Z]Z[0-9A-Z]$")] public string? GSTIN { get; set; }
    [RegularExpression(@"^$|^[A-Z]{5}[0-9]{4}[A-Z]$")] public string? PANNumber { get; set; }
    public string PhoneNumber { get; set; } = string.Empty; [EmailAddress] public string Email { get; set; } = string.Empty;
    [Required] public string BillingAddress { get; set; } = string.Empty; public string ShippingAddress { get; set; } = string.Empty;
    [Required] public string State { get; set; } = string.Empty; public string City { get; set; } = string.Empty; public string Pincode { get; set; } = string.Empty;
    public bool IsRegisteredGST { get; set; }
    public bool IsActive { get; set; } = true;
}

public class Supplier : AuditableEntity
{
    [Required, StringLength(200)] public string SupplierName { get; set; } = string.Empty;
    public string? GSTIN { get; set; }
    public string? PANNumber { get; set; }
    public string PhoneNumber { get; set; } = string.Empty;
    [EmailAddress] public string Email { get; set; } = string.Empty; [Required] public string Address { get; set; } = string.Empty;
    [Required] public string State { get; set; } = string.Empty; public string City { get; set; } = string.Empty; public string Pincode { get; set; } = string.Empty;
    public bool IsRegisteredGST { get; set; }
    public bool IsActive { get; set; } = true;
}

public class Product : AuditableEntity
{
    [Required] public string ProductName { get; set; } = string.Empty; [Required] public string ProductCode { get; set; } = string.Empty;
    [Required] public string HSNCode { get; set; } = string.Empty; [Range(0, 28)] public decimal GSTPercentage { get; set; }
    public string Unit { get; set; } = "Nos"; [Range(0, double.MaxValue)] public decimal SaleRate { get; set; }
    public decimal PurchaseRate { get; set; }
    public decimal OpeningStock { get; set; }
    public decimal CurrentStock { get; set; }
    public bool IsActive { get; set; } = true;
}

public class ServiceItem : AuditableEntity { [Required] public string ServiceName { get; set; } = string.Empty; [Required] public string SACCode { get; set; } = string.Empty; public decimal GSTPercentage { get; set; } public decimal Rate { get; set; } public bool IsActive { get; set; } = true; }
public class HsnSacCode : AuditableEntity { [Required] public string Code { get; set; } = string.Empty; public string Description { get; set; } = string.Empty; public string Type { get; set; } = "HSN"; public decimal GSTPercentage { get; set; } public bool IsActive { get; set; } = true; }
public class GstRate : AuditableEntity { public decimal GSTPercentage { get; set; } public string Description { get; set; } = string.Empty; public bool IsActive { get; set; } = true; }
public class InvoiceSeries : AuditableEntity { public string Prefix { get; set; } = "GST"; public int StartingNumber { get; set; } = 1; public int CurrentNumber { get; set; } = 0; public string FinancialYear { get; set; } = "2026-27"; public bool IsActive { get; set; } = true; public string NextNumber() => $"{Prefix}/{FinancialYear}/{(CurrentNumber + 1):0000}"; }

public class SalesInvoice : AuditableEntity
{
    [Required] public string InvoiceNumber { get; set; } = string.Empty; public DateTime InvoiceDate { get; set; } = DateTime.Today;
    public int CustomerId { get; set; }
    public Customer? Customer { get; set; }
    public string BillingAddress { get; set; } = string.Empty; public string ShippingAddress { get; set; } = string.Empty;
    public string PlaceOfSupply { get; set; } = string.Empty; public bool ReverseChargeApplicable { get; set; }
    public decimal TaxableAmount { get; set; }
    public decimal CGSTAmount { get; set; }
    public decimal SGSTAmount { get; set; }
    public decimal IGSTAmount { get; set; }
    public decimal TotalGSTAmount { get; set; }
    public decimal DiscountAmount { get; set; }
    public decimal RoundOff { get; set; }
    public decimal GrandTotal { get; set; }
    public string AmountInWords { get; set; } = string.Empty; public string? Notes { get; set; }
    public string? TermsAndConditions { get; set; }
    public string EInvoiceStatus { get; set; } = "Pending"; public string? IRNNumber { get; set; }
    public string? QRCode { get; set; }
    public string EWayBillStatus { get; set; } = "Pending"; public string? EWayBillNumber { get; set; }
    public List<SalesInvoiceItem> Items { get; set; } = [];
}

public class SalesInvoiceItem : AuditableEntity
{
    public int SalesInvoiceId { get; set; }
    public SalesInvoice? SalesInvoice { get; set; }
    public string ItemType { get; set; } = "Product"; public int? ProductId { get; set; }
    public int? ServiceItemId { get; set; }
    public string Description { get; set; } = string.Empty; public string HsnSac { get; set; } = string.Empty; public decimal Quantity { get; set; }
    public string Unit { get; set; } = string.Empty;
    public decimal Rate { get; set; }
    public decimal DiscountPercentage { get; set; }
    public decimal DiscountAmount { get; set; }
    public decimal TaxableAmount { get; set; }
    public decimal GSTPercentage { get; set; }
    public decimal CGSTPercentage { get; set; }
    public decimal CGSTAmount { get; set; }
    public decimal SGSTPercentage { get; set; }
    public decimal SGSTAmount { get; set; }
    public decimal IGSTPercentage { get; set; }
    public decimal IGSTAmount { get; set; }
    public decimal LineTotal { get; set; }
}

public class PurchaseInvoice : AuditableEntity
{
    public string PurchaseInvoiceNumber { get; set; } = string.Empty; public string SupplierInvoiceNumber { get; set; } = string.Empty; public DateTime InvoiceDate { get; set; } = DateTime.Today; public int SupplierId { get; set; }
    public Supplier? Supplier { get; set; }
    public decimal TaxableAmount { get; set; }
    public decimal CGSTAmount { get; set; }
    public decimal SGSTAmount { get; set; }
    public decimal IGSTAmount { get; set; }
    public decimal TotalGSTAmount { get; set; }
    public decimal DiscountAmount { get; set; }
    public decimal GrandTotal { get; set; }
    public string? Notes { get; set; }
    public decimal ITCEligibleAmount { get; set; }
    public List<PurchaseInvoiceItem> Items { get; set; } = [];
}
public class PurchaseInvoiceItem : AuditableEntity
{
    public int PurchaseInvoiceId { get; set; }
    public PurchaseInvoice? PurchaseInvoice { get; set; }
    public string Description { get; set; } = string.Empty; public string HsnSac { get; set; } = string.Empty; public decimal Quantity { get; set; }
    public decimal Rate { get; set; }
    public decimal TaxableAmount { get; set; }
    public decimal GSTPercentage { get; set; }
    public decimal CGSTAmount { get; set; }
    public decimal SGSTAmount { get; set; }
    public decimal IGSTAmount { get; set; }
    public decimal LineTotal { get; set; }
}
public class CreditNote : AuditableEntity
{
    public string CreditNoteNumber { get; set; } = string.Empty; public DateTime Date { get; set; } = DateTime.Today; public int CustomerId { get; set; }
    public int? ReferenceSalesInvoiceId { get; set; }
    public string Reason { get; set; } = string.Empty; public decimal TaxableAmount { get; set; }
    public decimal CGSTAmount { get; set; }
    public decimal SGSTAmount { get; set; }
    public decimal IGSTAmount { get; set; }
    public decimal GSTAmount { get; set; }
    public decimal GrandTotal { get; set; }
    public List<CreditNoteItem> Items { get; set; } = [];
}
public class CreditNoteItem : SalesInvoiceItem
{
    public int CreditNoteId { get; set; }
}
public class DebitNote : AuditableEntity
{
    public string DebitNoteNumber { get; set; } = string.Empty; public DateTime Date { get; set; } = DateTime.Today; public int SupplierId { get; set; }
    public int? ReferencePurchaseInvoiceId { get; set; }
    public string Reason { get; set; } = string.Empty; public decimal TaxableAmount { get; set; }
    public decimal CGSTAmount { get; set; }
    public decimal SGSTAmount { get; set; }
    public decimal IGSTAmount { get; set; }
    public decimal GSTAmount { get; set; }
    public decimal GrandTotal { get; set; }
    public List<DebitNoteItem> Items { get; set; } = [];
}
public class DebitNoteItem : PurchaseInvoiceItem
{
    public int DebitNoteId { get; set; }
}
public class Expense : AuditableEntity
{
    public DateTime ExpenseDate { get; set; } = DateTime.Today; public string ExpenseCategory { get; set; } = string.Empty; public string VendorName { get; set; } = string.Empty; public string? GSTIN { get; set; }
    public string InvoiceNumber { get; set; } = string.Empty; public decimal Amount { get; set; }
    public decimal GSTPercentage { get; set; }
    public decimal CGSTAmount { get; set; }
    public decimal SGSTAmount { get; set; }
    public decimal IGSTAmount { get; set; }
    public decimal TotalAmount { get; set; }
    public bool IsITCEligible { get; set; }
    public string? Notes { get; set; }
}
public class Receipt : AuditableEntity
{
    public string ReceiptNumber { get; set; } = string.Empty; public DateTime ReceiptDate { get; set; } = DateTime.Today; public int CustomerId { get; set; }
    public string PaymentMode { get; set; } = string.Empty; public decimal Amount { get; set; }
    public int? ReferenceInvoiceId { get; set; }
    public string? Notes { get; set; }
}
public class Payment : AuditableEntity
{
    public string PaymentNumber { get; set; } = string.Empty; public DateTime PaymentDate { get; set; } = DateTime.Today; public int SupplierId { get; set; }
    public string PaymentMode { get; set; } = string.Empty; public decimal Amount { get; set; }
    public int? ReferencePurchaseInvoiceId { get; set; }
    public string? Notes { get; set; }
}
public class GstLedgerEntry : AuditableEntity
{
    public string LedgerType { get; set; } = string.Empty; public DateTime TransactionDate { get; set; } = DateTime.Today; public string ReferenceType { get; set; } = string.Empty; public string ReferenceNumber { get; set; } = string.Empty; public decimal CGSTDebit { get; set; }
    public decimal CGSTCredit { get; set; }
    public decimal SGSTDebit { get; set; }
    public decimal SGSTCredit { get; set; }
    public decimal IGSTDebit { get; set; }
    public decimal IGSTCredit { get; set; }
    public decimal Balance { get; set; }
    public string Remarks { get; set; } = string.Empty;
}
public class InputTaxCreditLedger : GstLedgerEntry { }
public class OutputTaxLedger : GstLedgerEntry { }
public class GstLiabilityLedger : GstLedgerEntry { }
public class GstPaymentLedger : GstLedgerEntry { }
public class GstChallan : AuditableEntity
{
    public string ChallanNumber { get; set; } = string.Empty; public DateTime ChallanDate { get; set; } = DateTime.Today; public string Period { get; set; } = string.Empty; public decimal CGSTAmount { get; set; }
    public decimal SGSTAmount { get; set; }
    public decimal IGSTAmount { get; set; }
    public decimal Interest { get; set; }
    public decimal Penalty { get; set; }
    public decimal LateFee { get; set; }
    public decimal TotalAmount { get; set; }
    public string PaymentStatus { get; set; } = "Pending"; public string? PaymentReferenceNumber { get; set; }
    public string? Notes { get; set; }
}
public class GstPayment : AuditableEntity
{
    public DateTime PaymentDate { get; set; } = DateTime.Today;
    public int GstChallanId { get; set; }
    public string PaymentMode { get; set; } = string.Empty; public decimal Amount { get; set; }
    public string ReferenceNumber { get; set; } = string.Empty; public string Status { get; set; } = "Pending"; public string? Remarks { get; set; }
}
public class GstReturnFiling : AuditableEntity
{
    public string ReturnType { get; set; } = "GSTR-1";
    public int PeriodMonth { get; set; }
    public string FinancialYear { get; set; } = "2026-27"; public decimal TaxableAmount { get; set; }
    public decimal CGSTAmount { get; set; }
    public decimal SGSTAmount { get; set; }
    public decimal IGSTAmount { get; set; }
    public decimal TotalTaxAmount { get; set; }
    public string FilingStatus { get; set; } = "Draft"; public DateTime? FiledDate { get; set; }
    public string? AcknowledgementNumber { get; set; }
    public string? ErrorMessage { get; set; }
    public string? ApiResponse { get; set; }
}
public class GstReturnStatus : AuditableEntity
{
    public int GstReturnFilingId { get; set; }
    public string Status { get; set; } = "Pending"; public string? ApiResponse { get; set; }
    public DateTime CheckedDate { get; set; } = DateTime.UtcNow;
}
public class EInvoice : AuditableEntity
{
    public int SalesInvoiceId { get; set; }
    public string? IRNNumber { get; set; }
    public string? AcknowledgementNumber { get; set; }
    public DateTime? AcknowledgementDate { get; set; }
    public string? SignedInvoiceData { get; set; }
    public string? SignedQRCode { get; set; }
    public string Status { get; set; } = "Pending"; public string? ErrorMessage { get; set; }
}
public class EWayBill : AuditableEntity
{
    public int SalesInvoiceId { get; set; }
    public string? EWayBillNumber { get; set; }
    public DateTime? EWayBillDate { get; set; }
    public DateTime? ValidFrom { get; set; }
    public DateTime? ValidUntil { get; set; }
    public string? TransporterName { get; set; }
    public string? TransporterGSTIN { get; set; }
    public string? VehicleNumber { get; set; }
    public int Distance { get; set; }
    public string Status { get; set; } = "Pending"; public string? ErrorMessage { get; set; }
}
public class GstApiCredential : AuditableEntity
{
    public string ProviderName { get; set; } = string.Empty; public string BaseUrl { get; set; } = string.Empty; public string? ClientId { get; set; }
    public string? ClientSecret { get; set; }
    public string? Username { get; set; }
    public string? Password { get; set; }
    public string? GSTIN { get; set; }
    public string Environment { get; set; } = "Sandbox"; public bool IsActive { get; set; }
}
public class GstApiRequestLog : AuditableEntity
{
    public string ApiName { get; set; } = string.Empty; public string RequestUrl { get; set; } = string.Empty; public string RequestMethod { get; set; } = "POST"; public string? RequestBody { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow; public string? RelatedEntityType { get; set; }
    public int? RelatedEntityId { get; set; }
    public int RetryCount { get; set; }
}
public class GstApiResponseLog : AuditableEntity
{
    public int? GstApiRequestLogId { get; set; }
    public string ApiName { get; set; } = string.Empty; public string? ResponseBody { get; set; }
    public int StatusCode { get; set; }
    public bool Success { get; set; }
    public string? ErrorMessage { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow; public string? RelatedEntityType { get; set; }
    public int? RelatedEntityId { get; set; }
}
public class GstReconciliation : AuditableEntity
{
    public string Type { get; set; } = "GSTR-2A"; public string Period { get; set; } = string.Empty; public string Status { get; set; } = "Uploaded"; public string? UploadFilePath { get; set; }
    public List<GstReconciliationItem> Items { get; set; } = [];
}
public class GstReconciliationItem : AuditableEntity
{
    public int GstReconciliationId { get; set; }
    public string SupplierGSTIN { get; set; } = string.Empty; public string InvoiceNumber { get; set; } = string.Empty; public DateTime InvoiceDate { get; set; }
    public decimal InvoiceAmount { get; set; }
    public string MatchStatus { get; set; } = "Pending"; public string? Remarks { get; set; }
}
public class AuditLog : AuditableEntity
{
    public string ActionType { get; set; } = string.Empty; public string EntityName { get; set; } = string.Empty; public string EntityId { get; set; } = string.Empty; public string? OldValue { get; set; }
    public string? NewValue { get; set; }
    public string? UserId { get; set; }
    public string ModuleName { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? IpAddress { get; set; }
}
public class AppSetting : AuditableEntity
{
    public string Key { get; set; } = string.Empty; public string Value { get; set; } = string.Empty; public string? Description { get; set; }
}
public record GstApiOptions
{
    public string Provider { get; init; } = "MockGSP"; public string BaseUrl { get; init; } = string.Empty; public string Environment { get; init; } = "Sandbox"; public bool UseMock { get; init; } = true; public int MaxRetries { get; init; } = 3; public int FailureRatePercent { get; init; } = 15;
}
public record GstApiResult(bool Success, string Message, string? ReferenceNumber = null, string? Payload = null, int StatusCode = 200, bool ManualFallbackAvailable = false);
public record GstTaxBreakup(decimal TaxableAmount, decimal CGSTAmount, decimal SGSTAmount, decimal IGSTAmount, decimal TotalGSTAmount, decimal GrandTotal, decimal CGSTRate, decimal SGSTRate, decimal IGSTRate);
