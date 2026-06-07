using System;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

using AdvancedGSTApp.Data;

namespace AdvancedGSTApp.Migrations
{
    /// <inheritdoc />
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20260606093000_ConvertToMultiTenantSaaS")]
    public partial class ConvertToMultiTenantSaaS : Migration
    {
        private static readonly string[] TenantTables =
        {
            "CompanyProfiles", "Customers", "Suppliers", "Products", "ServiceItems", "HsnSacCodes", "GstRates", "InvoiceSeries",
            "SalesInvoices", "SalesInvoiceItems", "PurchaseInvoices", "PurchaseInvoiceItems", "CreditNotes", "DebitNotes",
            "Expenses", "Receipts", "Payments", "InputTaxCreditLedgers", "OutputTaxLedgers", "GstLiabilityLedgers", "GstPaymentLedgers", "GstChallans",
            "GstPayments", "GstReturnFilings", "GstReturnStatuses", "EInvoices", "EWayBills", "GstApiCredentials", "GstApiRequestLogs", "GstApiResponseLogs",
            "GstReconciliations", "GstReconciliationItems", "AuditLogs", "AppSettings"
        };

        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SubscriptionPlans",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false).Annotation("SqlServer:Identity", "1, 1"),
                    PlanName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MonthlyPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    YearlyPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
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
                constraints: table => table.PrimaryKey("PK_SubscriptionPlans", x => x.Id));

            migrationBuilder.CreateTable(
                name: "Tenants",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false).Annotation("SqlServer:Identity", "1, 1"),
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
                    table.ForeignKey("FK_Tenants_SubscriptionPlans_SubscriptionPlanId", x => x.SubscriptionPlanId, "SubscriptionPlans", "Id", onDelete: ReferentialAction.Restrict);
                });

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
        'Default professional plan for existing migrated data.',
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
END");

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
        CreatedBy,
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
        DATEADD(DAY, 365, SYSUTCDATETIME()),
        1,
        SYSUTCDATETIME(),
        DATEADD(DAY, 365, SYSUTCDATETIME()),
        SYSUTCDATETIME(),
        'Migration',
        0
    );

    SET IDENTITY_INSERT Tenants OFF;
END");

            migrationBuilder.CreateTable(
                name: "TenantSubscriptions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false).Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    SubscriptionPlanId = table.Column<int>(type: "int", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    BillingCycle = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PaymentStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PaymentReferenceNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TenantSubscriptions", x => x.Id);
                    table.ForeignKey("FK_TenantSubscriptions_SubscriptionPlans_SubscriptionPlanId", x => x.SubscriptionPlanId, "SubscriptionPlans", "Id", onDelete: ReferentialAction.Cascade);
                    table.ForeignKey("FK_TenantSubscriptions_Tenants_TenantId", x => x.TenantId, "Tenants", "Id", onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SaasPayments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false).Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    SubscriptionPlanId = table.Column<int>(type: "int", nullable: false),
                    TenantSubscriptionId = table.Column<int>(type: "int", nullable: true),
                    InvoiceNumber = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PaymentDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PaymentMode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PaymentStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TransactionReference = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SaasPayments", x => x.Id);
                    table.ForeignKey("FK_SaasPayments_SubscriptionPlans_SubscriptionPlanId", x => x.SubscriptionPlanId, "SubscriptionPlans", "Id", onDelete: ReferentialAction.Cascade);
                    table.ForeignKey("FK_SaasPayments_TenantSubscriptions_TenantSubscriptionId", x => x.TenantSubscriptionId, "TenantSubscriptions", "Id");
                    table.ForeignKey("FK_SaasPayments_Tenants_TenantId", x => x.TenantId, "Tenants", "Id", onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.DropIndex(name: "IX_RolePermissions_RoleId_ModuleName", table: "RolePermissions");

            migrationBuilder.AddColumn<int>(name: "TenantId", table: "AspNetUsers", type: "int", nullable: true);
            migrationBuilder.AddColumn<bool>(name: "IsPlatformUser", table: "AspNetUsers", type: "bit", nullable: false, defaultValue: false);
            migrationBuilder.AddColumn<int>(name: "TenantId", table: "AspNetRoles", type: "int", nullable: true);
            migrationBuilder.AddColumn<bool>(name: "IsPlatformRole", table: "AspNetRoles", type: "bit", nullable: false, defaultValue: false);
            migrationBuilder.AddColumn<int>(name: "TenantId", table: "RolePermissions", type: "int", nullable: true);

            foreach (var table in TenantTables)
            {
                migrationBuilder.AddColumn<int>(name: "TenantId", table: table, type: "int", nullable: false, defaultValue: 0);
            }

            foreach (var table in TenantTables)
            {
                migrationBuilder.Sql($"UPDATE {table} SET TenantId = 1 WHERE TenantId = 0;");
                migrationBuilder.AlterColumn<int>(name: "TenantId", table: table, type: "int", nullable: false, defaultValue: 1, oldClrType: typeof(int), oldType: "int", oldDefaultValue: 0);
            }

            migrationBuilder.Sql("UPDATE AspNetUsers SET IsPlatformUser = 1, TenantId = NULL WHERE NormalizedEmail = 'ADMIN@GSTSOFTWARE.COM'; UPDATE AspNetUsers SET TenantId = 1 WHERE TenantId IS NULL AND (IsPlatformUser = 0 OR IsPlatformUser IS NULL);");
            migrationBuilder.Sql("UPDATE AspNetRoles SET IsPlatformRole = 1, TenantId = NULL WHERE NormalizedName IN ('SUPER ADMIN','SUPPORT ADMIN'); UPDATE AspNetRoles SET TenantId = 1 WHERE TenantId IS NULL AND (IsPlatformRole = 0 OR IsPlatformRole IS NULL);");

            foreach (var table in TenantTables)
            {
                migrationBuilder.CreateIndex(name: $"IX_{table}_TenantId", table: table, column: "TenantId");
                migrationBuilder.AddForeignKey(name: $"FK_{table}_Tenants_TenantId", table: table, column: "TenantId", principalTable: "Tenants", principalColumn: "Id", onDelete: ReferentialAction.Restrict);
            }

            migrationBuilder.CreateIndex("IX_Tenants_Email", "Tenants", "Email", unique: true);
            migrationBuilder.CreateIndex("IX_Tenants_GSTIN", "Tenants", "GSTIN", unique: true);
            migrationBuilder.CreateIndex("IX_Tenants_SubscriptionPlanId", "Tenants", "SubscriptionPlanId");
            migrationBuilder.CreateIndex("IX_SubscriptionPlans_PlanName", "SubscriptionPlans", "PlanName", unique: true);
            migrationBuilder.CreateIndex("IX_TenantSubscriptions_TenantId_Status", "TenantSubscriptions", new[] { "TenantId", "Status" });
            migrationBuilder.CreateIndex("IX_TenantSubscriptions_SubscriptionPlanId", "TenantSubscriptions", "SubscriptionPlanId");
            migrationBuilder.CreateIndex("IX_SaasPayments_TenantId_InvoiceNumber", "SaasPayments", new[] { "TenantId", "InvoiceNumber" }, unique: true);
            migrationBuilder.CreateIndex("IX_SaasPayments_SubscriptionPlanId", "SaasPayments", "SubscriptionPlanId");
            migrationBuilder.CreateIndex("IX_SaasPayments_TenantSubscriptionId", "SaasPayments", "TenantSubscriptionId");
            migrationBuilder.CreateIndex("IX_AspNetUsers_TenantId", "AspNetUsers", "TenantId");
            migrationBuilder.CreateIndex("IX_AspNetRoles_TenantId", "AspNetRoles", "TenantId");
            migrationBuilder.CreateIndex("IX_RolePermissions_TenantId", "RolePermissions", "TenantId");
            migrationBuilder.CreateIndex("IX_RolePermissions_RoleId_ModuleName_TenantId", "RolePermissions", new[] { "RoleId", "ModuleName", "TenantId" }, unique: true, filter: "[TenantId] IS NOT NULL");
            migrationBuilder.AddForeignKey("FK_AspNetUsers_Tenants_TenantId", "AspNetUsers", "TenantId", "Tenants", principalColumn: "Id", onDelete: ReferentialAction.Restrict);
            migrationBuilder.AddForeignKey("FK_AspNetRoles_Tenants_TenantId", "AspNetRoles", "TenantId", "Tenants", principalColumn: "Id", onDelete: ReferentialAction.Restrict);
            migrationBuilder.AddForeignKey("FK_RolePermissions_Tenants_TenantId", "RolePermissions", "TenantId", "Tenants", principalColumn: "Id", onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            foreach (var table in TenantTables)
            {
                migrationBuilder.DropForeignKey(name: $"FK_{table}_Tenants_TenantId", table: table);
                migrationBuilder.DropIndex(name: $"IX_{table}_TenantId", table: table);
                migrationBuilder.DropColumn(name: "TenantId", table: table);
            }
            migrationBuilder.DropForeignKey("FK_AspNetUsers_Tenants_TenantId", "AspNetUsers");
            migrationBuilder.DropForeignKey("FK_AspNetRoles_Tenants_TenantId", "AspNetRoles");
            migrationBuilder.DropForeignKey("FK_RolePermissions_Tenants_TenantId", "RolePermissions");
            migrationBuilder.DropIndex(name: "IX_RolePermissions_RoleId_ModuleName_TenantId", table: "RolePermissions");
            migrationBuilder.CreateIndex(name: "IX_RolePermissions_RoleId_ModuleName", table: "RolePermissions", columns: new[] { "RoleId", "ModuleName" }, unique: true);
            migrationBuilder.DropColumn("TenantId", "AspNetUsers");
            migrationBuilder.DropColumn("IsPlatformUser", "AspNetUsers");
            migrationBuilder.DropColumn("TenantId", "AspNetRoles");
            migrationBuilder.DropColumn("IsPlatformRole", "AspNetRoles");
            migrationBuilder.DropColumn("TenantId", "RolePermissions");
            migrationBuilder.DropTable("SaasPayments");
            migrationBuilder.DropTable("TenantSubscriptions");
            migrationBuilder.DropTable("Tenants");
            migrationBuilder.DropTable("SubscriptionPlans");
        }
    }
}
