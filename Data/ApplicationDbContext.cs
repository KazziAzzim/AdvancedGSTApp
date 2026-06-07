using AdvancedGSTApp.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AdvancedGSTApp.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser, ApplicationRole, string>(options)
{
    public DbSet<CompanyProfile> CompanyProfiles => Set<CompanyProfile>();
    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<Supplier> Suppliers => Set<Supplier>();
    public DbSet<Product> Products => Set<Product>();
    public DbSet<ServiceItem> ServiceItems => Set<ServiceItem>();
    public DbSet<HsnSacCode> HsnSacCodes => Set<HsnSacCode>();
    public DbSet<GstRate> GstRates => Set<GstRate>();
    public DbSet<InvoiceSeries> InvoiceSeries => Set<InvoiceSeries>();
    public DbSet<SalesInvoice> SalesInvoices => Set<SalesInvoice>();
    public DbSet<SalesInvoiceItem> SalesInvoiceItems => Set<SalesInvoiceItem>();
    public DbSet<PurchaseInvoice> PurchaseInvoices => Set<PurchaseInvoice>();
    public DbSet<PurchaseInvoiceItem> PurchaseInvoiceItems => Set<PurchaseInvoiceItem>();
    public DbSet<CreditNote> CreditNotes => Set<CreditNote>();
    public DbSet<CreditNoteItem> CreditNoteItems => Set<CreditNoteItem>();
    public DbSet<DebitNote> DebitNotes => Set<DebitNote>();
    public DbSet<DebitNoteItem> DebitNoteItems => Set<DebitNoteItem>();
    public DbSet<Expense> Expenses => Set<Expense>();
    public DbSet<Receipt> Receipts => Set<Receipt>();
    public DbSet<Payment> Payments => Set<Payment>();
    public DbSet<InputTaxCreditLedger> InputTaxCreditLedgers => Set<InputTaxCreditLedger>();
    public DbSet<OutputTaxLedger> OutputTaxLedgers => Set<OutputTaxLedger>();
    public DbSet<GstLiabilityLedger> GstLiabilityLedgers => Set<GstLiabilityLedger>();
    public DbSet<GstPaymentLedger> GstPaymentLedgers => Set<GstPaymentLedger>();
    public DbSet<GstChallan> GstChallans => Set<GstChallan>();
    public DbSet<GstPayment> GstPayments => Set<GstPayment>();
    public DbSet<GstReturnFiling> GstReturnFilings => Set<GstReturnFiling>();
    public DbSet<GstReturnStatus> GstReturnStatuses => Set<GstReturnStatus>();
    public DbSet<EInvoice> EInvoices => Set<EInvoice>();
    public DbSet<EWayBill> EWayBills => Set<EWayBill>();
    public DbSet<GstApiCredential> GstApiCredentials => Set<GstApiCredential>();
    public DbSet<GstApiRequestLog> GstApiRequestLogs => Set<GstApiRequestLog>();
    public DbSet<GstApiResponseLog> GstApiResponseLogs => Set<GstApiResponseLog>();
    public DbSet<GstReconciliation> GstReconciliations => Set<GstReconciliation>();
    public DbSet<GstReconciliationItem> GstReconciliationItems => Set<GstReconciliationItem>();
    public DbSet<AuditLog> AuditLogs => Set<AuditLog>();
    public DbSet<RolePermission> RolePermissions => Set<RolePermission>();
    public DbSet<AppSetting> AppSettings => Set<AppSetting>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        foreach (var entityType in builder.Model.GetEntityTypes()
        .Where(e =>
        typeof(AuditableEntity).IsAssignableFrom(e.ClrType)
        && e.BaseType == null
        && !e.IsOwned()))
        {
            builder.Entity(entityType.ClrType)
                .HasQueryFilter(CreateSoftDeleteFilter(entityType.ClrType));
        }

        foreach (var property in builder.Model.GetEntityTypes().SelectMany(t => t.GetProperties()).Where(p => p.ClrType == typeof(decimal) || p.ClrType == typeof(decimal?)))
        {
            property.SetPrecision(18); property.SetScale(2);
        }
        ConfigureTenantDeleteBehavior(builder);

        builder.Entity<SalesInvoice>().HasMany(x => x.Items).WithOne(x => x.SalesInvoice).HasForeignKey(x => x.SalesInvoiceId).OnDelete(DeleteBehavior.Cascade);
        builder.Entity<PurchaseInvoice>().HasMany(x => x.Items).WithOne(x => x.PurchaseInvoice).HasForeignKey(x => x.PurchaseInvoiceId).OnDelete(DeleteBehavior.Cascade);
        builder.Entity<RolePermission>().HasIndex(x => new { x.RoleId, x.ModuleName }).IsUnique();
        builder.Entity<RolePermission>().HasOne(x => x.Role).WithMany().HasForeignKey(x => x.RoleId).OnDelete(DeleteBehavior.Cascade);
    }

    private static void ConfigureTenantDeleteBehavior(ModelBuilder builder)
    {
        var tenantType = builder.Model.GetEntityTypes()
            .FirstOrDefault(entityType => entityType.ClrType.Name == "Tenant")
            ?.ClrType;

        if (tenantType is null)
        {
            return;
        }

        foreach (var entityType in builder.Model.GetEntityTypes()
            .Where(entityType => typeof(ITenantEntity).IsAssignableFrom(entityType.ClrType))
            .ToList())
        {
            var tenantForeignKey = entityType.GetForeignKeys()
                .FirstOrDefault(foreignKey =>
                    foreignKey.PrincipalEntityType.ClrType == tenantType
                    && foreignKey.Properties.Any(property => property.Name == nameof(ITenantEntity.TenantId)));

            if (tenantForeignKey is not null)
            {
                tenantForeignKey.DeleteBehavior = DeleteBehavior.NoAction;
                continue;
            }

            builder.Entity(entityType.ClrType)
                .HasOne(tenantType, null)
                .WithMany()
                .HasForeignKey(nameof(ITenantEntity.TenantId))
                .OnDelete(DeleteBehavior.NoAction);
        }

        ConfigureIdentityTenantDeleteBehavior<ApplicationUser>(builder, tenantType);
        ConfigureIdentityTenantDeleteBehavior<ApplicationRole>(builder, tenantType);
    }

    private static void ConfigureIdentityTenantDeleteBehavior<TIdentityEntity>(ModelBuilder builder, Type tenantType)
        where TIdentityEntity : class
    {
        var entityType = builder.Model.FindEntityType(typeof(TIdentityEntity));
        if (entityType?.FindProperty(nameof(ITenantEntity.TenantId)) is null)
        {
            return;
        }

        var tenantForeignKey = entityType.GetForeignKeys()
            .FirstOrDefault(foreignKey =>
                foreignKey.PrincipalEntityType.ClrType == tenantType
                && foreignKey.Properties.Any(property => property.Name == nameof(ITenantEntity.TenantId)));

        if (tenantForeignKey is not null)
        {
            tenantForeignKey.DeleteBehavior = DeleteBehavior.Restrict;
            return;
        }

        builder.Entity<TIdentityEntity>()
            .HasOne(tenantType, null)
            .WithMany()
            .HasForeignKey(nameof(ITenantEntity.TenantId))
            .OnDelete(DeleteBehavior.Restrict);
    }

    private static System.Linq.Expressions.LambdaExpression CreateSoftDeleteFilter(Type entityType)
    {
        var parameter = System.Linq.Expressions.Expression.Parameter(entityType, "e");
        var property = System.Linq.Expressions.Expression.Property(parameter, nameof(AuditableEntity.IsDeleted));
        var condition = System.Linq.Expressions.Expression.Equal(property, System.Linq.Expressions.Expression.Constant(false));
        return System.Linq.Expressions.Expression.Lambda(condition, parameter);
    }
}
