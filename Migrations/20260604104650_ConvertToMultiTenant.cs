using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AdvancedGSTApp.Migrations
{
    /// <inheritdoc />
    public partial class ConvertToMultiTenant : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_RolePermissions_RoleId_ModuleName",
                table: "RolePermissions");

            migrationBuilder.AlterColumn<string>(
                name: "GSTIN",
                table: "Suppliers",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TenantId",
                table: "Suppliers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TenantId",
                table: "ServiceItems",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "InvoiceNumber",
                table: "SalesInvoices",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "FinancialYear",
                table: "SalesInvoices",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TenantId",
                table: "SalesInvoices",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TenantId",
                table: "SalesInvoiceItems",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TenantId",
                table: "RolePermissions",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TenantId",
                table: "Receipts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TenantId",
                table: "PurchaseInvoices",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TenantId",
                table: "PurchaseInvoiceItems",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "ProductCode",
                table: "Products",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<int>(
                name: "TenantId",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TenantId",
                table: "Payments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TenantId",
                table: "OutputTaxLedgers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "Prefix",
                table: "InvoiceSeries",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "FinancialYear",
                table: "InvoiceSeries",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<int>(
                name: "TenantId",
                table: "InvoiceSeries",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TenantId",
                table: "InputTaxCreditLedgers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TenantId",
                table: "HsnSacCodes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TenantId",
                table: "GstReturnStatuses",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TenantId",
                table: "GstReturnFilings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TenantId",
                table: "GstReconciliations",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TenantId",
                table: "GstReconciliationItems",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TenantId",
                table: "GstRates",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TenantId",
                table: "GstPayments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TenantId",
                table: "GstPaymentLedgers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TenantId",
                table: "GstLiabilityLedgers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TenantId",
                table: "GstChallans",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TenantId",
                table: "GstApiResponseLogs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TenantId",
                table: "GstApiRequestLogs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TenantId",
                table: "GstApiCredentials",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TenantId",
                table: "Expenses",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TenantId",
                table: "EWayBills",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TenantId",
                table: "EInvoices",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TenantId",
                table: "DebitNotes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "GSTIN",
                table: "Customers",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TenantId",
                table: "Customers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TenantId",
                table: "CreditNotes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TenantId",
                table: "CompanyProfiles",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TenantId",
                table: "AuditLogs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsPlatformUser",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "TenantId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsPlatformRole",
                table: "AspNetRoles",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "TenantId",
                table: "AspNetRoles",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TenantId",
                table: "AppSettings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "SubscriptionPlans",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PlanName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MonthlyPrice = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    YearlyPrice = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    MaxUsers = table.Column<int>(type: "int", nullable: false),
                    MaxInvoicesPerMonth = table.Column<int>(type: "int", nullable: false),
                    MaxCompanies = table.Column<int>(type: "int", nullable: false),
                    HasEInvoice = table.Column<bool>(type: "bit", nullable: false),
                    HasEWayBill = table.Column<bool>(type: "bit", nullable: false),
                    HasGstReconciliation = table.Column<bool>(type: "bit", nullable: false),
                    HasGstApiIntegration = table.Column<bool>(type: "bit", nullable: false),
                    HasAdvancedReports = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubscriptionPlans", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tenants",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    BusinessName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    GSTIN = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PANNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ContactPersonName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    State = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Pincode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Subdomain = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DatabaseName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LogoPath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsTrial = table.Column<bool>(type: "bit", nullable: false),
                    TrialStartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TrialEndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SubscriptionPlanId = table.Column<int>(type: "int", nullable: true),
                    SubscriptionStartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SubscriptionEndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tenants", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tenants_SubscriptionPlans_SubscriptionPlanId",
                        column: x => x.SubscriptionPlanId,
                        principalTable: "SubscriptionPlans",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "TenantSubscriptions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    SubscriptionPlanId = table.Column<int>(type: "int", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    BillingCycle = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Status = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PaymentStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PaymentReferenceNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TenantSubscriptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TenantSubscriptions_SubscriptionPlans_SubscriptionPlanId",
                        column: x => x.SubscriptionPlanId,
                        principalTable: "SubscriptionPlans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TenantSubscriptions_Tenants_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "SaasPayments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    SubscriptionPlanId = table.Column<int>(type: "int", nullable: false),
                    TenantSubscriptionId = table.Column<int>(type: "int", nullable: true),
                    InvoiceNumber = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PaymentDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    PaymentMode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PaymentStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TransactionReference = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SaasPayments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SaasPayments_SubscriptionPlans_SubscriptionPlanId",
                        column: x => x.SubscriptionPlanId,
                        principalTable: "SubscriptionPlans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SaasPayments_TenantSubscriptions_TenantSubscriptionId",
                        column: x => x.TenantSubscriptionId,
                        principalTable: "TenantSubscriptions",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SaasPayments_Tenants_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });
            // --------------------------------------------------------------------
            // Create default SaaS plan and default tenant before applying FK constraints.
            // Existing records received TenantId = 0 when TenantId columns were added.
            // TenantId = 0 is invalid because no tenant exists with Id = 0.
            // --------------------------------------------------------------------

            migrationBuilder.Sql(@"
IF NOT EXISTS (SELECT 1 FROM SubscriptionPlans WHERE Id = 1)
BEGIN
    SET IDENTITY_INSERT SubscriptionPlans ON;

    INSERT INTO SubscriptionPlans
    (
        Id,
        PlanName,
        Description,
        MonthlyPrice,
        YearlyPrice,
        MaxUsers,
        MaxInvoicesPerMonth,
        MaxCompanies,
        HasEInvoice,
        HasEWayBill,
        HasGstReconciliation,
        HasGstApiIntegration,
        HasAdvancedReports,
        IsActive,
        CreatedDate
    )
    VALUES
    (
        1,
        'Professional',
        'Default professional plan for migrated existing data.',
        0,
        0,
        15,
        1000,
        1,
        1,
        1,
        1,
        1,
        1,
        1,
        SYSUTCDATETIME()
    );

    SET IDENTITY_INSERT SubscriptionPlans OFF;
END
");

            migrationBuilder.Sql(@"
IF NOT EXISTS (SELECT 1 FROM Tenants WHERE Id = 1)
BEGIN
    SET IDENTITY_INSERT Tenants ON;

    INSERT INTO Tenants
    (
        Id,
        TenantName,
        BusinessName,
        GSTIN,
        PANNumber,
        ContactPersonName,
        Email,
        PhoneNumber,
        Address,
        State,
        City,
        Pincode,
        IsActive,
        IsTrial,
        TrialStartDate,
        TrialEndDate,
        SubscriptionPlanId,
        SubscriptionStartDate,
        SubscriptionEndDate,
        CreatedDate,
        IsDeleted
    )
    VALUES
    (
        1,
        'Demo GST Company',
        'Demo GST Company',
        '24ABCDE1234F1Z5',
        'ABCDE1234F',
        'Super Admin',
        'info@demogstcompany.com',
        '9876543210',
        '101, Business Plaza, Ring Road',
        'Gujarat',
        'Surat',
        '395003',
        1,
        0,
        SYSUTCDATETIME(),
        DATEADD(YEAR, 1, SYSUTCDATETIME()),
        1,
        SYSUTCDATETIME(),
        DATEADD(YEAR, 1, SYSUTCDATETIME()),
        SYSUTCDATETIME(),
        0
    );

    SET IDENTITY_INSERT Tenants OFF;
END
");

            migrationBuilder.Sql("UPDATE AppSettings SET TenantId = 1 WHERE TenantId = 0;");
            migrationBuilder.Sql("UPDATE CompanyProfiles SET TenantId = 1 WHERE TenantId = 0;");
            migrationBuilder.Sql("UPDATE Customers SET TenantId = 1 WHERE TenantId = 0;");
            migrationBuilder.Sql("UPDATE Suppliers SET TenantId = 1 WHERE TenantId = 0;");
            migrationBuilder.Sql("UPDATE Products SET TenantId = 1 WHERE TenantId = 0;");
            migrationBuilder.Sql("UPDATE ServiceItems SET TenantId = 1 WHERE TenantId = 0;");
            migrationBuilder.Sql("UPDATE HsnSacCodes SET TenantId = 1 WHERE TenantId = 0;");
            migrationBuilder.Sql("UPDATE GstRates SET TenantId = 1 WHERE TenantId = 0;");
            migrationBuilder.Sql("UPDATE InvoiceSeries SET TenantId = 1 WHERE TenantId = 0;");
            migrationBuilder.Sql("UPDATE SalesInvoices SET TenantId = 1 WHERE TenantId = 0;");
            migrationBuilder.Sql("UPDATE SalesInvoiceItems SET TenantId = 1 WHERE TenantId = 0;");
            migrationBuilder.Sql("UPDATE PurchaseInvoices SET TenantId = 1 WHERE TenantId = 0;");
            migrationBuilder.Sql("UPDATE PurchaseInvoiceItems SET TenantId = 1 WHERE TenantId = 0;");
            migrationBuilder.Sql("UPDATE CreditNotes SET TenantId = 1 WHERE TenantId = 0;");
            migrationBuilder.Sql("UPDATE DebitNotes SET TenantId = 1 WHERE TenantId = 0;");
            migrationBuilder.Sql("UPDATE Expenses SET TenantId = 1 WHERE TenantId = 0;");
            migrationBuilder.Sql("UPDATE Receipts SET TenantId = 1 WHERE TenantId = 0;");
            migrationBuilder.Sql("UPDATE Payments SET TenantId = 1 WHERE TenantId = 0;");
            migrationBuilder.Sql("UPDATE AuditLogs SET TenantId = 1 WHERE TenantId = 0;");
            migrationBuilder.Sql("UPDATE GstApiCredentials SET TenantId = 1 WHERE TenantId = 0;");
            migrationBuilder.Sql("UPDATE GstApiRequestLogs SET TenantId = 1 WHERE TenantId = 0;");
            migrationBuilder.Sql("UPDATE GstApiResponseLogs SET TenantId = 1 WHERE TenantId = 0;");
            migrationBuilder.Sql("UPDATE EInvoices SET TenantId = 1 WHERE TenantId = 0;");
            migrationBuilder.Sql("UPDATE EWayBills SET TenantId = 1 WHERE TenantId = 0;");
            migrationBuilder.Sql("UPDATE GstChallans SET TenantId = 1 WHERE TenantId = 0;");
            migrationBuilder.Sql("UPDATE GstPayments SET TenantId = 1 WHERE TenantId = 0;");
            migrationBuilder.Sql("UPDATE GstReturnFilings SET TenantId = 1 WHERE TenantId = 0;");
            migrationBuilder.Sql("UPDATE GstReturnStatuses SET TenantId = 1 WHERE TenantId = 0;");
            migrationBuilder.Sql("UPDATE GstReconciliations SET TenantId = 1 WHERE TenantId = 0;");
            migrationBuilder.Sql("UPDATE GstReconciliationItems SET TenantId = 1 WHERE TenantId = 0;");
            migrationBuilder.Sql("UPDATE InputTaxCreditLedgers SET TenantId = 1 WHERE TenantId = 0;");
            migrationBuilder.Sql("UPDATE OutputTaxLedgers SET TenantId = 1 WHERE TenantId = 0;");
            migrationBuilder.Sql("UPDATE GstLiabilityLedgers SET TenantId = 1 WHERE TenantId = 0;");
            migrationBuilder.Sql("UPDATE GstPaymentLedgers SET TenantId = 1 WHERE TenantId = 0;");
            migrationBuilder.CreateIndex(
                name: "IX_Suppliers_TenantId",
                table: "Suppliers",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_Suppliers_TenantId_GSTIN",
                table: "Suppliers",
                columns: new[] { "TenantId", "GSTIN" });

            migrationBuilder.CreateIndex(
                name: "IX_ServiceItems_TenantId",
                table: "ServiceItems",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_SalesInvoices_TenantId",
                table: "SalesInvoices",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_SalesInvoices_TenantId_InvoiceNumber_FinancialYear",
                table: "SalesInvoices",
                columns: new[] { "TenantId", "InvoiceNumber", "FinancialYear" },
                unique: true,
                filter: "[FinancialYear] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_SalesInvoiceItems_TenantId",
                table: "SalesInvoiceItems",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_RolePermissions_RoleId_ModuleName_TenantId",
                table: "RolePermissions",
                columns: new[] { "RoleId", "ModuleName", "TenantId" },
                unique: true,
                filter: "[TenantId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_RolePermissions_TenantId",
                table: "RolePermissions",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_Receipts_TenantId",
                table: "Receipts",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseInvoices_TenantId",
                table: "PurchaseInvoices",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseInvoiceItems_TenantId",
                table: "PurchaseInvoiceItems",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_TenantId",
                table: "Products",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_TenantId_ProductCode",
                table: "Products",
                columns: new[] { "TenantId", "ProductCode" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Payments_TenantId",
                table: "Payments",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_OutputTaxLedgers_TenantId",
                table: "OutputTaxLedgers",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceSeries_TenantId",
                table: "InvoiceSeries",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceSeries_TenantId_Prefix_FinancialYear",
                table: "InvoiceSeries",
                columns: new[] { "TenantId", "Prefix", "FinancialYear" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_InputTaxCreditLedgers_TenantId",
                table: "InputTaxCreditLedgers",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_HsnSacCodes_TenantId",
                table: "HsnSacCodes",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_GstReturnStatuses_TenantId",
                table: "GstReturnStatuses",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_GstReturnFilings_TenantId",
                table: "GstReturnFilings",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_GstReconciliations_TenantId",
                table: "GstReconciliations",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_GstReconciliationItems_TenantId",
                table: "GstReconciliationItems",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_GstRates_TenantId",
                table: "GstRates",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_GstPayments_TenantId",
                table: "GstPayments",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_GstPaymentLedgers_TenantId",
                table: "GstPaymentLedgers",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_GstLiabilityLedgers_TenantId",
                table: "GstLiabilityLedgers",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_GstChallans_TenantId",
                table: "GstChallans",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_GstApiResponseLogs_TenantId",
                table: "GstApiResponseLogs",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_GstApiRequestLogs_TenantId",
                table: "GstApiRequestLogs",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_GstApiCredentials_TenantId",
                table: "GstApiCredentials",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_Expenses_TenantId",
                table: "Expenses",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_EWayBills_TenantId",
                table: "EWayBills",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_EInvoices_TenantId",
                table: "EInvoices",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_DebitNotes_TenantId",
                table: "DebitNotes",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_Customers_TenantId",
                table: "Customers",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_Customers_TenantId_GSTIN",
                table: "Customers",
                columns: new[] { "TenantId", "GSTIN" });

            migrationBuilder.CreateIndex(
                name: "IX_CreditNotes_TenantId",
                table: "CreditNotes",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyProfiles_TenantId",
                table: "CompanyProfiles",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_AuditLogs_TenantId",
                table: "AuditLogs",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_TenantId",
                table: "AspNetUsers",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoles_TenantId",
                table: "AspNetRoles",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_AppSettings_TenantId",
                table: "AppSettings",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_SaasPayments_SubscriptionPlanId",
                table: "SaasPayments",
                column: "SubscriptionPlanId");

            migrationBuilder.CreateIndex(
                name: "IX_SaasPayments_TenantId_InvoiceNumber",
                table: "SaasPayments",
                columns: new[] { "TenantId", "InvoiceNumber" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SaasPayments_TenantSubscriptionId",
                table: "SaasPayments",
                column: "TenantSubscriptionId");

            migrationBuilder.CreateIndex(
                name: "IX_SubscriptionPlans_PlanName",
                table: "SubscriptionPlans",
                column: "PlanName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tenants_Email",
                table: "Tenants",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tenants_GSTIN",
                table: "Tenants",
                column: "GSTIN",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tenants_SubscriptionPlanId",
                table: "Tenants",
                column: "SubscriptionPlanId");

            migrationBuilder.CreateIndex(
                name: "IX_TenantSubscriptions_SubscriptionPlanId",
                table: "TenantSubscriptions",
                column: "SubscriptionPlanId");

            migrationBuilder.CreateIndex(
                name: "IX_TenantSubscriptions_TenantId_Status",
                table: "TenantSubscriptions",
                columns: new[] { "TenantId", "Status" });

            migrationBuilder.AddForeignKey(
                name: "FK_AppSettings_Tenants_TenantId",
                table: "AppSettings",
                column: "TenantId",
                principalTable: "Tenants",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetRoles_Tenants_TenantId",
                table: "AspNetRoles",
                column: "TenantId",
                principalTable: "Tenants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Tenants_TenantId",
                table: "AspNetUsers",
                column: "TenantId",
                principalTable: "Tenants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AuditLogs_Tenants_TenantId",
                table: "AuditLogs",
                column: "TenantId",
                principalTable: "Tenants",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_CompanyProfiles_Tenants_TenantId",
                table: "CompanyProfiles",
                column: "TenantId",
                principalTable: "Tenants",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_CreditNotes_Tenants_TenantId",
                table: "CreditNotes",
                column: "TenantId",
                principalTable: "Tenants",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Customers_Tenants_TenantId",
                table: "Customers",
                column: "TenantId",
                principalTable: "Tenants",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_DebitNotes_Tenants_TenantId",
                table: "DebitNotes",
                column: "TenantId",
                principalTable: "Tenants",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_EInvoices_Tenants_TenantId",
                table: "EInvoices",
                column: "TenantId",
                principalTable: "Tenants",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_EWayBills_Tenants_TenantId",
                table: "EWayBills",
                column: "TenantId",
                principalTable: "Tenants",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Expenses_Tenants_TenantId",
                table: "Expenses",
                column: "TenantId",
                principalTable: "Tenants",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_GstApiCredentials_Tenants_TenantId",
                table: "GstApiCredentials",
                column: "TenantId",
                principalTable: "Tenants",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_GstApiRequestLogs_Tenants_TenantId",
                table: "GstApiRequestLogs",
                column: "TenantId",
                principalTable: "Tenants",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_GstApiResponseLogs_Tenants_TenantId",
                table: "GstApiResponseLogs",
                column: "TenantId",
                principalTable: "Tenants",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_GstChallans_Tenants_TenantId",
                table: "GstChallans",
                column: "TenantId",
                principalTable: "Tenants",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_GstLiabilityLedgers_Tenants_TenantId",
                table: "GstLiabilityLedgers",
                column: "TenantId",
                principalTable: "Tenants",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_GstPaymentLedgers_Tenants_TenantId",
                table: "GstPaymentLedgers",
                column: "TenantId",
                principalTable: "Tenants",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_GstPayments_Tenants_TenantId",
                table: "GstPayments",
                column: "TenantId",
                principalTable: "Tenants",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_GstRates_Tenants_TenantId",
                table: "GstRates",
                column: "TenantId",
                principalTable: "Tenants",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_GstReconciliationItems_Tenants_TenantId",
                table: "GstReconciliationItems",
                column: "TenantId",
                principalTable: "Tenants",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_GstReconciliations_Tenants_TenantId",
                table: "GstReconciliations",
                column: "TenantId",
                principalTable: "Tenants",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_GstReturnFilings_Tenants_TenantId",
                table: "GstReturnFilings",
                column: "TenantId",
                principalTable: "Tenants",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_GstReturnStatuses_Tenants_TenantId",
                table: "GstReturnStatuses",
                column: "TenantId",
                principalTable: "Tenants",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_HsnSacCodes_Tenants_TenantId",
                table: "HsnSacCodes",
                column: "TenantId",
                principalTable: "Tenants",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_InputTaxCreditLedgers_Tenants_TenantId",
                table: "InputTaxCreditLedgers",
                column: "TenantId",
                principalTable: "Tenants",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_InvoiceSeries_Tenants_TenantId",
                table: "InvoiceSeries",
                column: "TenantId",
                principalTable: "Tenants",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_OutputTaxLedgers_Tenants_TenantId",
                table: "OutputTaxLedgers",
                column: "TenantId",
                principalTable: "Tenants",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_Tenants_TenantId",
                table: "Payments",
                column: "TenantId",
                principalTable: "Tenants",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Tenants_TenantId",
                table: "Products",
                column: "TenantId",
                principalTable: "Tenants",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseInvoiceItems_Tenants_TenantId",
                table: "PurchaseInvoiceItems",
                column: "TenantId",
                principalTable: "Tenants",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseInvoices_Tenants_TenantId",
                table: "PurchaseInvoices",
                column: "TenantId",
                principalTable: "Tenants",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Receipts_Tenants_TenantId",
                table: "Receipts",
                column: "TenantId",
                principalTable: "Tenants",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_RolePermissions_Tenants_TenantId",
                table: "RolePermissions",
                column: "TenantId",
                principalTable: "Tenants",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SalesInvoiceItems_Tenants_TenantId",
                table: "SalesInvoiceItems",
                column: "TenantId",
                principalTable: "Tenants",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_SalesInvoices_Tenants_TenantId",
                table: "SalesInvoices",
                column: "TenantId",
                principalTable: "Tenants",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceItems_Tenants_TenantId",
                table: "ServiceItems",
                column: "TenantId",
                principalTable: "Tenants",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Suppliers_Tenants_TenantId",
                table: "Suppliers",
                column: "TenantId",
                principalTable: "Tenants",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppSettings_Tenants_TenantId",
                table: "AppSettings");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetRoles_Tenants_TenantId",
                table: "AspNetRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Tenants_TenantId",
                table: "AspNetUsers");

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
                name: "FK_RolePermissions_Tenants_TenantId",
                table: "RolePermissions");

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

            migrationBuilder.DropTable(
                name: "SaasPayments");

            migrationBuilder.DropTable(
                name: "TenantSubscriptions");

            migrationBuilder.DropTable(
                name: "Tenants");

            migrationBuilder.DropTable(
                name: "SubscriptionPlans");

            migrationBuilder.DropIndex(
                name: "IX_Suppliers_TenantId",
                table: "Suppliers");

            migrationBuilder.DropIndex(
                name: "IX_Suppliers_TenantId_GSTIN",
                table: "Suppliers");

            migrationBuilder.DropIndex(
                name: "IX_ServiceItems_TenantId",
                table: "ServiceItems");

            migrationBuilder.DropIndex(
                name: "IX_SalesInvoices_TenantId",
                table: "SalesInvoices");

            migrationBuilder.DropIndex(
                name: "IX_SalesInvoices_TenantId_InvoiceNumber_FinancialYear",
                table: "SalesInvoices");

            migrationBuilder.DropIndex(
                name: "IX_SalesInvoiceItems_TenantId",
                table: "SalesInvoiceItems");

            migrationBuilder.DropIndex(
                name: "IX_RolePermissions_RoleId_ModuleName_TenantId",
                table: "RolePermissions");

            migrationBuilder.DropIndex(
                name: "IX_RolePermissions_TenantId",
                table: "RolePermissions");

            migrationBuilder.DropIndex(
                name: "IX_Receipts_TenantId",
                table: "Receipts");

            migrationBuilder.DropIndex(
                name: "IX_PurchaseInvoices_TenantId",
                table: "PurchaseInvoices");

            migrationBuilder.DropIndex(
                name: "IX_PurchaseInvoiceItems_TenantId",
                table: "PurchaseInvoiceItems");

            migrationBuilder.DropIndex(
                name: "IX_Products_TenantId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_TenantId_ProductCode",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Payments_TenantId",
                table: "Payments");

            migrationBuilder.DropIndex(
                name: "IX_OutputTaxLedgers_TenantId",
                table: "OutputTaxLedgers");

            migrationBuilder.DropIndex(
                name: "IX_InvoiceSeries_TenantId",
                table: "InvoiceSeries");

            migrationBuilder.DropIndex(
                name: "IX_InvoiceSeries_TenantId_Prefix_FinancialYear",
                table: "InvoiceSeries");

            migrationBuilder.DropIndex(
                name: "IX_InputTaxCreditLedgers_TenantId",
                table: "InputTaxCreditLedgers");

            migrationBuilder.DropIndex(
                name: "IX_HsnSacCodes_TenantId",
                table: "HsnSacCodes");

            migrationBuilder.DropIndex(
                name: "IX_GstReturnStatuses_TenantId",
                table: "GstReturnStatuses");

            migrationBuilder.DropIndex(
                name: "IX_GstReturnFilings_TenantId",
                table: "GstReturnFilings");

            migrationBuilder.DropIndex(
                name: "IX_GstReconciliations_TenantId",
                table: "GstReconciliations");

            migrationBuilder.DropIndex(
                name: "IX_GstReconciliationItems_TenantId",
                table: "GstReconciliationItems");

            migrationBuilder.DropIndex(
                name: "IX_GstRates_TenantId",
                table: "GstRates");

            migrationBuilder.DropIndex(
                name: "IX_GstPayments_TenantId",
                table: "GstPayments");

            migrationBuilder.DropIndex(
                name: "IX_GstPaymentLedgers_TenantId",
                table: "GstPaymentLedgers");

            migrationBuilder.DropIndex(
                name: "IX_GstLiabilityLedgers_TenantId",
                table: "GstLiabilityLedgers");

            migrationBuilder.DropIndex(
                name: "IX_GstChallans_TenantId",
                table: "GstChallans");

            migrationBuilder.DropIndex(
                name: "IX_GstApiResponseLogs_TenantId",
                table: "GstApiResponseLogs");

            migrationBuilder.DropIndex(
                name: "IX_GstApiRequestLogs_TenantId",
                table: "GstApiRequestLogs");

            migrationBuilder.DropIndex(
                name: "IX_GstApiCredentials_TenantId",
                table: "GstApiCredentials");

            migrationBuilder.DropIndex(
                name: "IX_Expenses_TenantId",
                table: "Expenses");

            migrationBuilder.DropIndex(
                name: "IX_EWayBills_TenantId",
                table: "EWayBills");

            migrationBuilder.DropIndex(
                name: "IX_EInvoices_TenantId",
                table: "EInvoices");

            migrationBuilder.DropIndex(
                name: "IX_DebitNotes_TenantId",
                table: "DebitNotes");

            migrationBuilder.DropIndex(
                name: "IX_Customers_TenantId",
                table: "Customers");

            migrationBuilder.DropIndex(
                name: "IX_Customers_TenantId_GSTIN",
                table: "Customers");

            migrationBuilder.DropIndex(
                name: "IX_CreditNotes_TenantId",
                table: "CreditNotes");

            migrationBuilder.DropIndex(
                name: "IX_CompanyProfiles_TenantId",
                table: "CompanyProfiles");

            migrationBuilder.DropIndex(
                name: "IX_AuditLogs_TenantId",
                table: "AuditLogs");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_TenantId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetRoles_TenantId",
                table: "AspNetRoles");

            migrationBuilder.DropIndex(
                name: "IX_AppSettings_TenantId",
                table: "AppSettings");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "Suppliers");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "ServiceItems");

            migrationBuilder.DropColumn(
                name: "FinancialYear",
                table: "SalesInvoices");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "SalesInvoices");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "SalesInvoiceItems");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "RolePermissions");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "Receipts");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "PurchaseInvoices");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "PurchaseInvoiceItems");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "OutputTaxLedgers");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "InvoiceSeries");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "InputTaxCreditLedgers");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "HsnSacCodes");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "GstReturnStatuses");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "GstReturnFilings");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "GstReconciliations");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "GstReconciliationItems");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "GstRates");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "GstPayments");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "GstPaymentLedgers");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "GstLiabilityLedgers");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "GstChallans");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "GstApiResponseLogs");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "GstApiRequestLogs");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "GstApiCredentials");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "Expenses");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "EWayBills");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "EInvoices");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "DebitNotes");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "CreditNotes");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "CompanyProfiles");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "AuditLogs");

            migrationBuilder.DropColumn(
                name: "IsPlatformUser",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "IsPlatformRole",
                table: "AspNetRoles");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "AspNetRoles");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "AppSettings");

            migrationBuilder.AlterColumn<string>(
                name: "GSTIN",
                table: "Suppliers",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "InvoiceNumber",
                table: "SalesInvoices",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "ProductCode",
                table: "Products",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "Prefix",
                table: "InvoiceSeries",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "FinancialYear",
                table: "InvoiceSeries",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "GSTIN",
                table: "Customers",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_RolePermissions_RoleId_ModuleName",
                table: "RolePermissions",
                columns: new[] { "RoleId", "ModuleName" },
                unique: true);
        }
    }
}
