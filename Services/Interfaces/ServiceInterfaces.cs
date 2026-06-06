using AdvancedGSTApp.Models;

namespace AdvancedGSTApp.Services.Interfaces;

public interface IGstCalculationService { GstTaxBreakup Calculate(decimal quantity, decimal rate, decimal discountPercent, decimal gstPercent, string companyState, string partyState); void Recalculate(SalesInvoice invoice, string companyState); }
public interface ISalesInvoiceService { Task<List<SalesInvoice>> GetAllAsync(); Task<SalesInvoice?> GetByIdAsync(int id); Task<SalesInvoice> CreateAsync(SalesInvoice invoice); Task DeleteAsync(int id); Task<string> GenerateNextInvoiceNumberAsync(); }
public interface IPurchaseInvoiceService { Task<List<PurchaseInvoice>> GetAllAsync(); Task<PurchaseInvoice?> GetByIdAsync(int id); Task<PurchaseInvoice> SaveAsync(PurchaseInvoice invoice); }
public interface IGstReportService { Task<DashboardViewModel> GetDashboardAsync(); Task<GstPayableSummaryViewModel> GetGstPayableSummaryAsync(DateTime? from, DateTime? to); Task<List<Gstr1RowViewModel>> GetGstr1Async(int? month, string? financialYear); Task<Gstr3BViewModel> GetGstr3BAsync(int? month, string? financialYear); }
public interface IGstReturnService { Task<GstReturnFiling> PrepareAsync(string returnType, int month, string financialYear); Task<GstApiResult> SubmitAsync(int returnFilingId); }
public interface IGstApiService
{
    Task<GstApiResult> ValidateGstinAsync(string gstin); Task<GstApiResult> ValidatePanAsync(string pan); Task<GstApiResult> ValidateHsnSacAsync(string code);
    Task<GstApiResult> GenerateEInvoiceAsync(int salesInvoiceId); Task<GstApiResult> CancelEInvoiceAsync(int eInvoiceId); Task<GstApiResult> GenerateEWayBillAsync(int salesInvoiceId); Task<GstApiResult> CancelEWayBillAsync(int eWayBillId);
    Task<GstApiResult> GenerateGstChallanAsync(int challanId); Task<GstApiResult> SubmitGstr1Async(int returnFilingId); Task<GstApiResult> SubmitGstr3BAsync(int returnFilingId); Task<GstApiResult> GetReturnStatusAsync(int returnFilingId); Task<GstApiResult> GetPaymentStatusAsync(int gstPaymentId);
}
public interface IEInvoiceService { Task<GstApiResult> GenerateAsync(int salesInvoiceId); Task<GstApiResult> CancelAsync(int eInvoiceId); }
public interface IEWayBillService { Task<GstApiResult> GenerateAsync(int salesInvoiceId); Task<GstApiResult> CancelAsync(int eWayBillId); }
public interface IGstChallanService { Task<GstChallan> SaveAsync(GstChallan challan); Task<GstApiResult> GenerateAsync(int challanId); }
public interface IGstReconciliationService { Task<GstReconciliation> ImportAsync(Stream stream, string fileName, string type, string period); Task<List<GstReconciliationItem>> MatchAsync(int reconciliationId); }
public interface IPdfService { Task<byte[]> GenerateInvoicePdfAsync(int salesInvoiceId); }
public interface IEmailService { Task SendInvoiceAsync(int salesInvoiceId, string toEmail); }
public interface IAuditLogService { Task LogAsync(string actionType, string entityName, string entityId, object? oldValue, object? newValue); }
