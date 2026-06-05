using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AdvancedGSTApp.Migrations
{
    /// <inheritdoc />
    public partial class add : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppSettings_Tenants_TenantId",
                table: "AppSettings");

            migrationBuilder.DropForeignKey(
                name: "FK_AuditLogs_Tenants_TenantId",
                table: "AuditLogs");

            migrationBuilder.DropForeignKey(
                name: "FK_CompanyProfiles_Tenants_TenantId",
                table: "CompanyProfiles");

            migrationBuilder.DropForeignKey(
                name: "FK_CreditNotes_Tenants_TenantId",
                table: "CreditNotes");

            migrationBuilder.DropForeignKey(
                name: "FK_Customers_Tenants_TenantId",
                table: "Customers");

            migrationBuilder.DropForeignKey(
                name: "FK_DebitNotes_Tenants_TenantId",
                table: "DebitNotes");

            migrationBuilder.DropForeignKey(
                name: "FK_EInvoices_Tenants_TenantId",
                table: "EInvoices");

            migrationBuilder.DropForeignKey(
                name: "FK_EWayBills_Tenants_TenantId",
                table: "EWayBills");

            migrationBuilder.DropForeignKey(
                name: "FK_Expenses_Tenants_TenantId",
                table: "Expenses");

            migrationBuilder.DropForeignKey(
                name: "FK_GstApiCredentials_Tenants_TenantId",
                table: "GstApiCredentials");

            migrationBuilder.DropForeignKey(
                name: "FK_GstApiRequestLogs_Tenants_TenantId",
                table: "GstApiRequestLogs");

            migrationBuilder.DropForeignKey(
                name: "FK_GstApiResponseLogs_Tenants_TenantId",
                table: "GstApiResponseLogs");

            migrationBuilder.DropForeignKey(
                name: "FK_GstChallans_Tenants_TenantId",
                table: "GstChallans");

            migrationBuilder.DropForeignKey(
                name: "FK_GstLiabilityLedgers_Tenants_TenantId",
                table: "GstLiabilityLedgers");

            migrationBuilder.DropForeignKey(
                name: "FK_GstPaymentLedgers_Tenants_TenantId",
                table: "GstPaymentLedgers");

            migrationBuilder.DropForeignKey(
                name: "FK_GstPayments_Tenants_TenantId",
                table: "GstPayments");

            migrationBuilder.DropForeignKey(
                name: "FK_GstRates_Tenants_TenantId",
                table: "GstRates");

            migrationBuilder.DropForeignKey(
                name: "FK_GstReconciliationItems_Tenants_TenantId",
                table: "GstReconciliationItems");

            migrationBuilder.DropForeignKey(
                name: "FK_GstReconciliations_Tenants_TenantId",
                table: "GstReconciliations");

            migrationBuilder.DropForeignKey(
                name: "FK_GstReturnFilings_Tenants_TenantId",
                table: "GstReturnFilings");

            migrationBuilder.DropForeignKey(
                name: "FK_GstReturnStatuses_Tenants_TenantId",
                table: "GstReturnStatuses");

            migrationBuilder.DropForeignKey(
                name: "FK_HsnSacCodes_Tenants_TenantId",
                table: "HsnSacCodes");

            migrationBuilder.DropForeignKey(
                name: "FK_InputTaxCreditLedgers_Tenants_TenantId",
                table: "InputTaxCreditLedgers");

            migrationBuilder.DropForeignKey(
                name: "FK_InvoiceSeries_Tenants_TenantId",
                table: "InvoiceSeries");

            migrationBuilder.DropForeignKey(
                name: "FK_OutputTaxLedgers_Tenants_TenantId",
                table: "OutputTaxLedgers");

            migrationBuilder.DropForeignKey(
                name: "FK_Payments_Tenants_TenantId",
                table: "Payments");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_Tenants_TenantId",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_PurchaseInvoiceItems_Tenants_TenantId",
                table: "PurchaseInvoiceItems");

            migrationBuilder.DropForeignKey(
                name: "FK_PurchaseInvoices_Tenants_TenantId",
                table: "PurchaseInvoices");

            migrationBuilder.DropForeignKey(
                name: "FK_Receipts_Tenants_TenantId",
                table: "Receipts");

            migrationBuilder.DropForeignKey(
                name: "FK_SalesInvoiceItems_Tenants_TenantId",
                table: "SalesInvoiceItems");

            migrationBuilder.DropForeignKey(
                name: "FK_SalesInvoices_Tenants_TenantId",
                table: "SalesInvoices");

            migrationBuilder.DropForeignKey(
                name: "FK_ServiceItems_Tenants_TenantId",
                table: "ServiceItems");

            migrationBuilder.DropForeignKey(
                name: "FK_Suppliers_Tenants_TenantId",
                table: "Suppliers");

            migrationBuilder.AddForeignKey(
                name: "FK_AppSettings_Tenants_TenantId",
                table: "AppSettings",
                column: "TenantId",
                principalTable: "Tenants",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AuditLogs_Tenants_TenantId",
                table: "AuditLogs",
                column: "TenantId",
                principalTable: "Tenants",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CompanyProfiles_Tenants_TenantId",
                table: "CompanyProfiles",
                column: "TenantId",
                principalTable: "Tenants",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CreditNotes_Tenants_TenantId",
                table: "CreditNotes",
                column: "TenantId",
                principalTable: "Tenants",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Customers_Tenants_TenantId",
                table: "Customers",
                column: "TenantId",
                principalTable: "Tenants",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DebitNotes_Tenants_TenantId",
                table: "DebitNotes",
                column: "TenantId",
                principalTable: "Tenants",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_EInvoices_Tenants_TenantId",
                table: "EInvoices",
                column: "TenantId",
                principalTable: "Tenants",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_EWayBills_Tenants_TenantId",
                table: "EWayBills",
                column: "TenantId",
                principalTable: "Tenants",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Expenses_Tenants_TenantId",
                table: "Expenses",
                column: "TenantId",
                principalTable: "Tenants",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_GstApiCredentials_Tenants_TenantId",
                table: "GstApiCredentials",
                column: "TenantId",
                principalTable: "Tenants",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_GstApiRequestLogs_Tenants_TenantId",
                table: "GstApiRequestLogs",
                column: "TenantId",
                principalTable: "Tenants",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_GstApiResponseLogs_Tenants_TenantId",
                table: "GstApiResponseLogs",
                column: "TenantId",
                principalTable: "Tenants",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_GstChallans_Tenants_TenantId",
                table: "GstChallans",
                column: "TenantId",
                principalTable: "Tenants",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_GstLiabilityLedgers_Tenants_TenantId",
                table: "GstLiabilityLedgers",
                column: "TenantId",
                principalTable: "Tenants",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_GstPaymentLedgers_Tenants_TenantId",
                table: "GstPaymentLedgers",
                column: "TenantId",
                principalTable: "Tenants",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_GstPayments_Tenants_TenantId",
                table: "GstPayments",
                column: "TenantId",
                principalTable: "Tenants",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_GstRates_Tenants_TenantId",
                table: "GstRates",
                column: "TenantId",
                principalTable: "Tenants",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_GstReconciliationItems_Tenants_TenantId",
                table: "GstReconciliationItems",
                column: "TenantId",
                principalTable: "Tenants",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_GstReconciliations_Tenants_TenantId",
                table: "GstReconciliations",
                column: "TenantId",
                principalTable: "Tenants",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_GstReturnFilings_Tenants_TenantId",
                table: "GstReturnFilings",
                column: "TenantId",
                principalTable: "Tenants",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_GstReturnStatuses_Tenants_TenantId",
                table: "GstReturnStatuses",
                column: "TenantId",
                principalTable: "Tenants",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_HsnSacCodes_Tenants_TenantId",
                table: "HsnSacCodes",
                column: "TenantId",
                principalTable: "Tenants",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_InputTaxCreditLedgers_Tenants_TenantId",
                table: "InputTaxCreditLedgers",
                column: "TenantId",
                principalTable: "Tenants",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_InvoiceSeries_Tenants_TenantId",
                table: "InvoiceSeries",
                column: "TenantId",
                principalTable: "Tenants",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_OutputTaxLedgers_Tenants_TenantId",
                table: "OutputTaxLedgers",
                column: "TenantId",
                principalTable: "Tenants",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_Tenants_TenantId",
                table: "Payments",
                column: "TenantId",
                principalTable: "Tenants",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Tenants_TenantId",
                table: "Products",
                column: "TenantId",
                principalTable: "Tenants",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseInvoiceItems_Tenants_TenantId",
                table: "PurchaseInvoiceItems",
                column: "TenantId",
                principalTable: "Tenants",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseInvoices_Tenants_TenantId",
                table: "PurchaseInvoices",
                column: "TenantId",
                principalTable: "Tenants",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Receipts_Tenants_TenantId",
                table: "Receipts",
                column: "TenantId",
                principalTable: "Tenants",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SalesInvoiceItems_Tenants_TenantId",
                table: "SalesInvoiceItems",
                column: "TenantId",
                principalTable: "Tenants",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SalesInvoices_Tenants_TenantId",
                table: "SalesInvoices",
                column: "TenantId",
                principalTable: "Tenants",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceItems_Tenants_TenantId",
                table: "ServiceItems",
                column: "TenantId",
                principalTable: "Tenants",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Suppliers_Tenants_TenantId",
                table: "Suppliers",
                column: "TenantId",
                principalTable: "Tenants",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppSettings_Tenants_TenantId",
                table: "AppSettings");

            migrationBuilder.DropForeignKey(
                name: "FK_AuditLogs_Tenants_TenantId",
                table: "AuditLogs");

            migrationBuilder.DropForeignKey(
                name: "FK_CompanyProfiles_Tenants_TenantId",
                table: "CompanyProfiles");

            migrationBuilder.DropForeignKey(
                name: "FK_CreditNotes_Tenants_TenantId",
                table: "CreditNotes");

            migrationBuilder.DropForeignKey(
                name: "FK_Customers_Tenants_TenantId",
                table: "Customers");

            migrationBuilder.DropForeignKey(
                name: "FK_DebitNotes_Tenants_TenantId",
                table: "DebitNotes");

            migrationBuilder.DropForeignKey(
                name: "FK_EInvoices_Tenants_TenantId",
                table: "EInvoices");

            migrationBuilder.DropForeignKey(
                name: "FK_EWayBills_Tenants_TenantId",
                table: "EWayBills");

            migrationBuilder.DropForeignKey(
                name: "FK_Expenses_Tenants_TenantId",
                table: "Expenses");

            migrationBuilder.DropForeignKey(
                name: "FK_GstApiCredentials_Tenants_TenantId",
                table: "GstApiCredentials");

            migrationBuilder.DropForeignKey(
                name: "FK_GstApiRequestLogs_Tenants_TenantId",
                table: "GstApiRequestLogs");

            migrationBuilder.DropForeignKey(
                name: "FK_GstApiResponseLogs_Tenants_TenantId",
                table: "GstApiResponseLogs");

            migrationBuilder.DropForeignKey(
                name: "FK_GstChallans_Tenants_TenantId",
                table: "GstChallans");

            migrationBuilder.DropForeignKey(
                name: "FK_GstLiabilityLedgers_Tenants_TenantId",
                table: "GstLiabilityLedgers");

            migrationBuilder.DropForeignKey(
                name: "FK_GstPaymentLedgers_Tenants_TenantId",
                table: "GstPaymentLedgers");

            migrationBuilder.DropForeignKey(
                name: "FK_GstPayments_Tenants_TenantId",
                table: "GstPayments");

            migrationBuilder.DropForeignKey(
                name: "FK_GstRates_Tenants_TenantId",
                table: "GstRates");

            migrationBuilder.DropForeignKey(
                name: "FK_GstReconciliationItems_Tenants_TenantId",
                table: "GstReconciliationItems");

            migrationBuilder.DropForeignKey(
                name: "FK_GstReconciliations_Tenants_TenantId",
                table: "GstReconciliations");

            migrationBuilder.DropForeignKey(
                name: "FK_GstReturnFilings_Tenants_TenantId",
                table: "GstReturnFilings");

            migrationBuilder.DropForeignKey(
                name: "FK_GstReturnStatuses_Tenants_TenantId",
                table: "GstReturnStatuses");

            migrationBuilder.DropForeignKey(
                name: "FK_HsnSacCodes_Tenants_TenantId",
                table: "HsnSacCodes");

            migrationBuilder.DropForeignKey(
                name: "FK_InputTaxCreditLedgers_Tenants_TenantId",
                table: "InputTaxCreditLedgers");

            migrationBuilder.DropForeignKey(
                name: "FK_InvoiceSeries_Tenants_TenantId",
                table: "InvoiceSeries");

            migrationBuilder.DropForeignKey(
                name: "FK_OutputTaxLedgers_Tenants_TenantId",
                table: "OutputTaxLedgers");

            migrationBuilder.DropForeignKey(
                name: "FK_Payments_Tenants_TenantId",
                table: "Payments");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_Tenants_TenantId",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_PurchaseInvoiceItems_Tenants_TenantId",
                table: "PurchaseInvoiceItems");

            migrationBuilder.DropForeignKey(
                name: "FK_PurchaseInvoices_Tenants_TenantId",
                table: "PurchaseInvoices");

            migrationBuilder.DropForeignKey(
                name: "FK_Receipts_Tenants_TenantId",
                table: "Receipts");

            migrationBuilder.DropForeignKey(
                name: "FK_SalesInvoiceItems_Tenants_TenantId",
                table: "SalesInvoiceItems");

            migrationBuilder.DropForeignKey(
                name: "FK_SalesInvoices_Tenants_TenantId",
                table: "SalesInvoices");

            migrationBuilder.DropForeignKey(
                name: "FK_ServiceItems_Tenants_TenantId",
                table: "ServiceItems");

            migrationBuilder.DropForeignKey(
                name: "FK_Suppliers_Tenants_TenantId",
                table: "Suppliers");

            migrationBuilder.AddForeignKey(
                name: "FK_AppSettings_Tenants_TenantId",
                table: "AppSettings",
                column: "TenantId",
                principalTable: "Tenants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AuditLogs_Tenants_TenantId",
                table: "AuditLogs",
                column: "TenantId",
                principalTable: "Tenants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CompanyProfiles_Tenants_TenantId",
                table: "CompanyProfiles",
                column: "TenantId",
                principalTable: "Tenants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CreditNotes_Tenants_TenantId",
                table: "CreditNotes",
                column: "TenantId",
                principalTable: "Tenants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Customers_Tenants_TenantId",
                table: "Customers",
                column: "TenantId",
                principalTable: "Tenants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DebitNotes_Tenants_TenantId",
                table: "DebitNotes",
                column: "TenantId",
                principalTable: "Tenants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_EInvoices_Tenants_TenantId",
                table: "EInvoices",
                column: "TenantId",
                principalTable: "Tenants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_EWayBills_Tenants_TenantId",
                table: "EWayBills",
                column: "TenantId",
                principalTable: "Tenants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Expenses_Tenants_TenantId",
                table: "Expenses",
                column: "TenantId",
                principalTable: "Tenants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GstApiCredentials_Tenants_TenantId",
                table: "GstApiCredentials",
                column: "TenantId",
                principalTable: "Tenants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GstApiRequestLogs_Tenants_TenantId",
                table: "GstApiRequestLogs",
                column: "TenantId",
                principalTable: "Tenants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GstApiResponseLogs_Tenants_TenantId",
                table: "GstApiResponseLogs",
                column: "TenantId",
                principalTable: "Tenants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GstChallans_Tenants_TenantId",
                table: "GstChallans",
                column: "TenantId",
                principalTable: "Tenants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GstLiabilityLedgers_Tenants_TenantId",
                table: "GstLiabilityLedgers",
                column: "TenantId",
                principalTable: "Tenants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GstPaymentLedgers_Tenants_TenantId",
                table: "GstPaymentLedgers",
                column: "TenantId",
                principalTable: "Tenants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GstPayments_Tenants_TenantId",
                table: "GstPayments",
                column: "TenantId",
                principalTable: "Tenants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GstRates_Tenants_TenantId",
                table: "GstRates",
                column: "TenantId",
                principalTable: "Tenants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GstReconciliationItems_Tenants_TenantId",
                table: "GstReconciliationItems",
                column: "TenantId",
                principalTable: "Tenants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GstReconciliations_Tenants_TenantId",
                table: "GstReconciliations",
                column: "TenantId",
                principalTable: "Tenants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GstReturnFilings_Tenants_TenantId",
                table: "GstReturnFilings",
                column: "TenantId",
                principalTable: "Tenants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GstReturnStatuses_Tenants_TenantId",
                table: "GstReturnStatuses",
                column: "TenantId",
                principalTable: "Tenants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_HsnSacCodes_Tenants_TenantId",
                table: "HsnSacCodes",
                column: "TenantId",
                principalTable: "Tenants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_InputTaxCreditLedgers_Tenants_TenantId",
                table: "InputTaxCreditLedgers",
                column: "TenantId",
                principalTable: "Tenants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_InvoiceSeries_Tenants_TenantId",
                table: "InvoiceSeries",
                column: "TenantId",
                principalTable: "Tenants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OutputTaxLedgers_Tenants_TenantId",
                table: "OutputTaxLedgers",
                column: "TenantId",
                principalTable: "Tenants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_Tenants_TenantId",
                table: "Payments",
                column: "TenantId",
                principalTable: "Tenants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Tenants_TenantId",
                table: "Products",
                column: "TenantId",
                principalTable: "Tenants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseInvoiceItems_Tenants_TenantId",
                table: "PurchaseInvoiceItems",
                column: "TenantId",
                principalTable: "Tenants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseInvoices_Tenants_TenantId",
                table: "PurchaseInvoices",
                column: "TenantId",
                principalTable: "Tenants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Receipts_Tenants_TenantId",
                table: "Receipts",
                column: "TenantId",
                principalTable: "Tenants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SalesInvoiceItems_Tenants_TenantId",
                table: "SalesInvoiceItems",
                column: "TenantId",
                principalTable: "Tenants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SalesInvoices_Tenants_TenantId",
                table: "SalesInvoices",
                column: "TenantId",
                principalTable: "Tenants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceItems_Tenants_TenantId",
                table: "ServiceItems",
                column: "TenantId",
                principalTable: "Tenants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Suppliers_Tenants_TenantId",
                table: "Suppliers",
                column: "TenantId",
                principalTable: "Tenants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
