using AdvancedGSTApp.Data;
using AdvancedGSTApp.Models;
using AdvancedGSTApp.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace AdvancedGSTApp.Services;

public class MockGstApiService(ApplicationDbContext db, IOptions<GstApiOptions> options) : IGstApiService
{
    private readonly Random _random = new();
    private async Task<GstApiResult> ExecuteAsync(string api, object request, string relatedType, int? relatedId, Func<GstApiResult> success)
    {
        var opt = options.Value; GstApiRequestLog? log = null; GstApiResult result = new(false, "Not executed", StatusCode: 500, ManualFallbackAvailable: true);
        for (var retry = 0; retry <= opt.MaxRetries; retry++)
        {
            log = new GstApiRequestLog { ApiName = api, RequestUrl = $"{opt.BaseUrl}/{api}", RequestBody = JsonSerializer.Serialize(request), RelatedEntityType = relatedType, RelatedEntityId = relatedId, RetryCount = retry };
            db.GstApiRequestLogs.Add(log); await db.SaveChangesAsync();
            var failed = _random.Next(100) < opt.FailureRatePercent;
            result = failed ? new GstApiResult(false, $"Mock {api} failure after retry {retry}", StatusCode: 503, ManualFallbackAvailable: true) : success();
            db.GstApiResponseLogs.Add(new GstApiResponseLog { GstApiRequestLogId = log.Id, ApiName = api, ResponseBody = result.Payload ?? JsonSerializer.Serialize(result), StatusCode = result.StatusCode, Success = result.Success, ErrorMessage = result.Success ? null : result.Message, RelatedEntityType = relatedType, RelatedEntityId = relatedId });
            await db.SaveChangesAsync(); if (result.Success) break;
        }
        return result;
    }
    public Task<GstApiResult> ValidateGstinAsync(string gstin) => ExecuteAsync("validate-gstin", new { gstin }, "GSTIN", null, () => new(true, "GSTIN validated", gstin, JsonSerializer.Serialize(new { legalName = "Demo Taxpayer", gstin, status = "Active" })));
    public Task<GstApiResult> ValidatePanAsync(string pan) => ExecuteAsync("validate-pan", new { pan }, "PAN", null, () => new(true, "PAN validated", pan));
    public Task<GstApiResult> ValidateHsnSacAsync(string code) => ExecuteAsync("validate-hsn-sac", new { code }, "HSNSAC", null, () => new(true, "HSN/SAC validated", code));
    public Task<GstApiResult> GenerateEInvoiceAsync(int salesInvoiceId) => ExecuteAsync("e-invoice/generate", new { salesInvoiceId }, "SalesInvoice", salesInvoiceId, () => new(true, "E-invoice generated", $"IRN{DateTime.UtcNow:yyyyMMddHHmmss}{salesInvoiceId}", JsonSerializer.Serialize(new { irn = $"IRN{Guid.NewGuid():N}", ackNo = $"ACK{Random.Shared.Next(100000, 999999)}", qrCode = Convert.ToBase64String(Guid.NewGuid().ToByteArray()) })));
    public Task<GstApiResult> CancelEInvoiceAsync(int eInvoiceId) => ExecuteAsync("e-invoice/cancel", new { eInvoiceId }, "EInvoice", eInvoiceId, () => new(true, "E-invoice cancelled", $"CANCEL-{eInvoiceId}"));
    public Task<GstApiResult> GenerateEWayBillAsync(int salesInvoiceId) => ExecuteAsync("ewaybill/generate", new { salesInvoiceId }, "SalesInvoice", salesInvoiceId, () => new(true, "E-way bill generated", $"EWB{Random.Shared.NextInt64(100000000000, 999999999999)}"));
    public Task<GstApiResult> CancelEWayBillAsync(int eWayBillId) => ExecuteAsync("ewaybill/cancel", new { eWayBillId }, "EWayBill", eWayBillId, () => new(true, "E-way bill cancelled", $"EWBCANCEL-{eWayBillId}"));
    public Task<GstApiResult> GenerateGstChallanAsync(int challanId) => ExecuteAsync("challan/generate", new { challanId }, "GstChallan", challanId, () => new(true, "GST challan generated", $"CPIN{Random.Shared.NextInt64(10000000000000, 99999999999999)}"));
    public Task<GstApiResult> SubmitGstr1Async(int returnFilingId) => ExecuteAsync("returns/gstr1", new { returnFilingId }, "GstReturnFiling", returnFilingId, () => new(true, "GSTR-1 filed", $"ARN{DateTime.UtcNow:yyyyMMdd}{Random.Shared.Next(1000,9999)}"));
    public Task<GstApiResult> SubmitGstr3BAsync(int returnFilingId) => ExecuteAsync("returns/gstr3b", new { returnFilingId }, "GstReturnFiling", returnFilingId, () => new(true, "GSTR-3B filed", $"ARN{DateTime.UtcNow:yyyyMMdd}{Random.Shared.Next(1000,9999)}"));
    public Task<GstApiResult> GetReturnStatusAsync(int returnFilingId) => ExecuteAsync("returns/status", new { returnFilingId }, "GstReturnFiling", returnFilingId, () => new(true, "Return status fetched", "Filed"));
    public Task<GstApiResult> GetPaymentStatusAsync(int gstPaymentId) => ExecuteAsync("payment/status", new { gstPaymentId }, "GstPayment", gstPaymentId, () => new(true, "Payment status fetched", "Paid"));
}

public class RealGstApiService : IGstApiService
{
    private static Task<GstApiResult> NotConfigured() => Task.FromResult(new GstApiResult(false, "Real GST API integration placeholder: configure ASP/GSP SDK, credential encryption, auth token refresh, signing and production endpoints before enabling.", StatusCode: 501, ManualFallbackAvailable: true));
    public Task<GstApiResult> ValidateGstinAsync(string gstin) => NotConfigured(); public Task<GstApiResult> ValidatePanAsync(string pan) => NotConfigured(); public Task<GstApiResult> ValidateHsnSacAsync(string code) => NotConfigured(); public Task<GstApiResult> GenerateEInvoiceAsync(int salesInvoiceId) => NotConfigured(); public Task<GstApiResult> CancelEInvoiceAsync(int eInvoiceId) => NotConfigured(); public Task<GstApiResult> GenerateEWayBillAsync(int salesInvoiceId) => NotConfigured(); public Task<GstApiResult> CancelEWayBillAsync(int eWayBillId) => NotConfigured(); public Task<GstApiResult> GenerateGstChallanAsync(int challanId) => NotConfigured(); public Task<GstApiResult> SubmitGstr1Async(int returnFilingId) => NotConfigured(); public Task<GstApiResult> SubmitGstr3BAsync(int returnFilingId) => NotConfigured(); public Task<GstApiResult> GetReturnStatusAsync(int returnFilingId) => NotConfigured(); public Task<GstApiResult> GetPaymentStatusAsync(int gstPaymentId) => NotConfigured();
}

public class EInvoiceService(ApplicationDbContext db, IGstApiService api) : IEInvoiceService
{ public async Task<GstApiResult> GenerateAsync(int id) { var r = await api.GenerateEInvoiceAsync(id); var inv = await db.SalesInvoices.FindAsync(id); if (inv != null) { inv.EInvoiceStatus = r.Success ? "Generated" : "Failed"; inv.IRNNumber = r.ReferenceNumber; inv.QRCode = r.Payload; db.EInvoices.Add(new EInvoice { SalesInvoiceId = id, IRNNumber = r.ReferenceNumber, AcknowledgementNumber = r.Success ? $"ACK{id}" : null, AcknowledgementDate = r.Success ? DateTime.UtcNow : null, SignedInvoiceData = r.Payload, SignedQRCode = r.Payload, Status = inv.EInvoiceStatus, ErrorMessage = r.Success ? null : r.Message }); await db.SaveChangesAsync(); } return r; } public Task<GstApiResult> CancelAsync(int eInvoiceId) => api.CancelEInvoiceAsync(eInvoiceId); }
public class EWayBillService(ApplicationDbContext db, IGstApiService api) : IEWayBillService
{ public async Task<GstApiResult> GenerateAsync(int id) { var r = await api.GenerateEWayBillAsync(id); var inv = await db.SalesInvoices.FindAsync(id); if (inv != null) { inv.EWayBillStatus = r.Success ? "Generated" : "Failed"; inv.EWayBillNumber = r.ReferenceNumber; db.EWayBills.Add(new EWayBill { SalesInvoiceId = id, EWayBillNumber = r.ReferenceNumber, EWayBillDate = DateTime.UtcNow, ValidFrom = DateTime.UtcNow, ValidUntil = DateTime.UtcNow.AddDays(1), Status = inv.EWayBillStatus, ErrorMessage = r.Success ? null : r.Message }); await db.SaveChangesAsync(); } return r; } public Task<GstApiResult> CancelAsync(int eWayBillId) => api.CancelEWayBillAsync(eWayBillId); }
