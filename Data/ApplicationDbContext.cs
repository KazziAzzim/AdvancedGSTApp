using AdvancedGSTApp.Models;
using AdvancedGSTApp.Services.Interfaces;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace AdvancedGSTApp.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
{
    private readonly ITenantContext? tenantContext;

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, ITenantContext? tenantContext = null) : base(options)
    {
        this.tenantContext = tenantContext;
    }

    public int? CurrentTenantId => tenantContext?.TenantId;
    public bool IsTenantFilterDisabled => tenantContext is null || tenantContext.IsSuperAdmin || !tenantContext.HasTenant;

    public DbSet<Tenant> Tenants => Set<Tenant>();
    public DbSet<SubscriptionPlan> SubscriptionPlans => Set<SubscriptionPlan>();
    public DbSet<TenantSubscription> TenantSubscriptions => Set<TenantSubscription>();
    public DbSet<SaasPayment> SaasPayments => Set<SaasPayment>();
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
            .Where(e => typeof(ITenantEntity).IsAssignableFrom(e.ClrType) && e.BaseType == null && !e.IsOwned()))
        {
            builder.Entity(entityType.ClrType).HasQueryFilter(CreateTenantFilter(entityType.ClrType));
            builder.Entity(entityType.ClrType).HasIndex(nameof(ITenantEntity.TenantId));

            builder.Entity(entityType.ClrType)
                .HasOne(typeof(Tenant), nameof(TenantEntity.Tenant))
                .WithMany()
                .HasForeignKey(nameof(ITenantEntity.TenantId))
                .OnDelete(DeleteBehavior.NoAction);
        }

        foreach (var property in builder.Model.GetEntityTypes().SelectMany(t => t.GetProperties()).Where(p => p.ClrType == typeof(decimal) || p.ClrType == typeof(decimal?)))
        {
            property.SetPrecision(18); property.SetScale(2);
        }

        builder.Entity<Tenant>().HasQueryFilter(x => !x.IsDeleted);
        builder.Entity<Tenant>().HasIndex(x => x.Email).IsUnique();
        builder.Entity<Tenant>().HasIndex(x => x.GSTIN).IsUnique();
        builder.Entity<SubscriptionPlan>().HasIndex(x => x.PlanName).IsUnique();
        builder.Entity<TenantSubscription>().HasIndex(x => new { x.TenantId, x.Status });
        builder.Entity<SaasPayment>().HasIndex(x => new { x.TenantId, x.InvoiceNumber }).IsUnique();

        builder.Entity<ApplicationUser>().HasOne(x => x.Tenant).WithMany().HasForeignKey(x => x.TenantId).OnDelete(DeleteBehavior.Restrict);
        builder.Entity<ApplicationRole>().HasOne(x => x.Tenant).WithMany().HasForeignKey(x => x.TenantId).OnDelete(DeleteBehavior.Restrict);

        builder.Entity<SalesInvoice>().HasMany(x => x.Items).WithOne(x => x.SalesInvoice).HasForeignKey(x => x.SalesInvoiceId).OnDelete(DeleteBehavior.Cascade);
        builder.Entity<PurchaseInvoice>().HasMany(x => x.Items).WithOne(x => x.PurchaseInvoice).HasForeignKey(x => x.PurchaseInvoiceId).OnDelete(DeleteBehavior.Cascade);
        builder.Entity<RolePermission>().HasIndex(x => new { x.RoleId, x.ModuleName, x.TenantId }).IsUnique();
        builder.Entity<RolePermission>().HasOne(x => x.Role).WithMany().HasForeignKey(x => x.RoleId).OnDelete(DeleteBehavior.Cascade);
        builder.Entity<Customer>().HasIndex(x => new { x.TenantId, x.GSTIN });
        builder.Entity<Supplier>().HasIndex(x => new { x.TenantId, x.GSTIN });
        builder.Entity<Product>().HasIndex(x => new { x.TenantId, x.ProductCode }).IsUnique();
        builder.Entity<InvoiceSeries>().HasIndex(x => new { x.TenantId, x.Prefix, x.FinancialYear }).IsUnique();
        builder.Entity<SalesInvoice>().HasIndex(x => new { x.TenantId, x.InvoiceNumber, x.FinancialYear }).IsUnique();
    }

    public override int SaveChanges()
    {
        ApplyTenantAndAuditRules();
        return base.SaveChanges();
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        ApplyTenantAndAuditRules();
        return base.SaveChangesAsync(cancellationToken);
    }

    private LambdaExpression CreateTenantFilter(Type entityType)
    {
        var parameter = Expression.Parameter(entityType, "e");
        var isDeleted = Expression.Equal(Expression.Property(parameter, nameof(TenantEntity.IsDeleted)), Expression.Constant(false));
        var tenantId = Expression.Convert(Expression.Property(parameter, nameof(ITenantEntity.TenantId)), typeof(int?));
        var currentTenant = Expression.Property(Expression.Constant(this), nameof(CurrentTenantId));
        var filterDisabled = Expression.Property(Expression.Constant(this), nameof(IsTenantFilterDisabled));
        var tenantMatches = Expression.Equal(tenantId, currentTenant);
        var tenantCondition = Expression.OrElse(filterDisabled, tenantMatches);
        return Expression.Lambda(Expression.AndAlso(isDeleted, tenantCondition), parameter);
    }

    private void ApplyTenantAndAuditRules()
    {
        var tenantId = tenantContext?.TenantId;
        var isSuperAdmin = tenantContext?.IsSuperAdmin == true;
        var now = DateTime.UtcNow;

        foreach (var entry in ChangeTracker.Entries<TenantEntity>())
        {
            if (entry.State == EntityState.Added)
            {
                if (!isSuperAdmin && tenantId.HasValue)
                {
                    if (entry.Entity.TenantId != 0 && entry.Entity.TenantId != tenantId.Value)
                    {
                        throw new InvalidOperationException("Tenant users cannot create data for another tenant.");
                    }

                    entry.Entity.TenantId = tenantId.Value;
                }

                if (entry.Entity.TenantId == 0 && tenantId.HasValue)
                {
                    entry.Entity.TenantId = tenantId.Value;
                }
                else if (entry.Entity.TenantId == 0 && tenantContext is not null)
                {
                    entry.Entity.TenantId = Tenants.IgnoreQueryFilters().OrderBy(x => x.Id).Select(x => x.Id).FirstOrDefault();
                }

                entry.Entity.CreatedDate = now;
            }

            if (entry.State == EntityState.Modified)
            {
                if (!isSuperAdmin && tenantId.HasValue && entry.Entity.TenantId != tenantId.Value)
                {
                    throw new InvalidOperationException("Tenant users cannot modify data for another tenant.");
                }

                entry.Property(x => x.TenantId).IsModified = false;
                entry.Entity.UpdatedDate = now;
            }

            if (entry.State == EntityState.Deleted)
            {
                entry.State = EntityState.Modified;
                entry.Entity.IsDeleted = true;
                entry.Entity.UpdatedDate = now;
            }
        }
    }
}
