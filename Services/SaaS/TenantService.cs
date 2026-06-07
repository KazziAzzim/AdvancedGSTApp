using AdvancedGSTApp.Data;
using AdvancedGSTApp.Models;
using AdvancedGSTApp.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AdvancedGSTApp.Services.SaaS;

public class TenantService(
    ApplicationDbContext db,
    UserManager<ApplicationUser> userManager,
    RoleManager<ApplicationRole> roleManager) : ITenantService
{
    public async Task<Tenant> CreateTenantAsync(Tenant tenant, string adminEmail, string adminPassword)
    {
        var plan = await db.SubscriptionPlans.FirstOrDefaultAsync(x => x.PlanName == "Trial") ?? await db.SubscriptionPlans.FirstAsync(x => x.IsActive);
        tenant.TenantName = string.IsNullOrWhiteSpace(tenant.TenantName) ? tenant.BusinessName : tenant.TenantName;
        tenant.SubscriptionPlanId = plan.Id;
        tenant.TrialStartDate = DateTime.UtcNow;
        tenant.TrialEndDate = DateTime.UtcNow.AddDays(14);
        tenant.SubscriptionStartDate = tenant.TrialStartDate;
        tenant.SubscriptionEndDate = tenant.TrialEndDate;
        tenant.IsTrial = true;
        db.Tenants.Add(tenant);
        await db.SaveChangesAsync();

        db.TenantSubscriptions.Add(new TenantSubscription
        {
            TenantId = tenant.Id,
            SubscriptionPlanId = plan.Id,
            StartDate = tenant.SubscriptionStartDate.Value,
            EndDate = tenant.SubscriptionEndDate.Value,
            BillingCycle = "Trial",
            Status = "Trial",
            PaymentStatus = "Pending"
        });

        await EnsureTenantRoleAsync("Tenant Admin", tenant.Id);
        await EnsureTenantDefaultsAsync(tenant);
        await db.SaveChangesAsync();

        var user = new ApplicationUser
        {
            TenantId = tenant.Id,
            FullName = tenant.ContactPersonName,
            UserName = adminEmail,
            Email = adminEmail,
            PhoneNumber = tenant.PhoneNumber,
            EmailConfirmed = true,
            IsActive = true,
            IsPlatformUser = false,
            CreatedDate = DateTime.UtcNow
        };
        var result = await userManager.CreateAsync(user, adminPassword);
        if (!result.Succeeded)
        {
            throw new InvalidOperationException(string.Join(", ", result.Errors.Select(x => x.Description)));
        }

        await userManager.AddToRoleAsync(user, "Tenant Admin");
        return tenant;
    }

    public async Task<bool> ActivateAsync(int tenantId)
    {
        var tenant = await db.Tenants.IgnoreQueryFilters().FirstOrDefaultAsync(x => x.Id == tenantId && !x.IsDeleted);
        if (tenant is null) return false;
        tenant.IsActive = true;
        tenant.UpdatedDate = DateTime.UtcNow;
        await db.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeactivateAsync(int tenantId)
    {
        var tenant = await db.Tenants.IgnoreQueryFilters().FirstOrDefaultAsync(x => x.Id == tenantId && !x.IsDeleted);
        if (tenant is null) return false;
        tenant.IsActive = false;
        tenant.UpdatedDate = DateTime.UtcNow;
        await db.SaveChangesAsync();
        return true;
    }

    private async Task EnsureTenantRoleAsync(string roleName, int tenantId)
    {
        if (!await roleManager.RoleExistsAsync(roleName))
        {
            await roleManager.CreateAsync(new ApplicationRole { Name = roleName, IsPlatformRole = false, Description = "Tenant administrator role template." });
        }
    }

    private Task EnsureTenantDefaultsAsync(Tenant tenant)
    {
        db.CompanyProfiles.Add(new CompanyProfile
        {
            TenantId = tenant.Id,
            CompanyName = tenant.BusinessName,
            GSTIN = tenant.GSTIN,
            PANNumber = tenant.PANNumber,
            Address = tenant.Address,
            State = tenant.State,
            City = tenant.City,
            Pincode = tenant.Pincode,
            PhoneNumber = tenant.PhoneNumber,
            Email = tenant.Email,
            DefaultInvoicePrefix = "GST",
            FinancialYear = "2026-27",
            IsActive = true
        });
        foreach (var rate in new[] { 0m, 5m, 12m, 18m, 28m })
        {
            db.GstRates.Add(new GstRate { TenantId = tenant.Id, GSTPercentage = rate, Description = $"GST {rate:0}%", IsActive = true });
        }
        db.InvoiceSeries.Add(new InvoiceSeries { TenantId = tenant.Id, Prefix = "GST", FinancialYear = "2026-27", StartingNumber = 1, CurrentNumber = 1, IsActive = true });
        foreach (var setting in new[]
        {
            new AppSetting { TenantId = tenant.Id, Key = "FinancialYear", Value = "2026-27", Description = "Tenant financial year." },
            new AppSetting { TenantId = tenant.Id, Key = "DefaultGstRate", Value = "18", Description = "Tenant default GST rate." },
            new AppSetting { TenantId = tenant.Id, Key = "DefaultState", Value = tenant.State, Description = "Tenant default state." }
        })
        {
            db.AppSettings.Add(setting);
        }
        return Task.CompletedTask;
    }
}
