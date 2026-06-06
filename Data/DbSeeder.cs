using AdvancedGSTApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AdvancedGSTApp.Data;

public static class DbSeeder
{
    public static async Task SeedAsync(IServiceProvider services)
    {
        var db = services.GetRequiredService<ApplicationDbContext>();
        if (db.Database.IsRelational()) await db.Database.EnsureCreatedAsync();
        var roleManager = services.GetRequiredService<RoleManager<ApplicationRole>>();
        var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
        foreach (var role in new[] { "Super Admin", "Admin", "Accountant", "Staff" }) if (!await roleManager.RoleExistsAsync(role)) await roleManager.CreateAsync(new ApplicationRole { Name = role, Description = $"{role} role" });
        var adminEmail = "admin@advancedgst.local";
        var admin = await userManager.FindByEmailAsync(adminEmail);
        if (admin is null)
        {
            admin = new ApplicationUser { UserName = adminEmail, Email = adminEmail, EmailConfirmed = true, FullName = "Default Super Admin" };
            await userManager.CreateAsync(admin, "Admin@12345"); await userManager.AddToRoleAsync(admin, "Super Admin");
        }
        if (!await db.GstRates.AnyAsync()) { db.GstRates.AddRange(new[] { 0m, 5m, 12m, 18m, 28m }.Select(r => new GstRate { GSTPercentage = r, Description = $"GST {r}%", IsActive = true })); }
        if (!await db.InvoiceSeries.AnyAsync()) db.InvoiceSeries.Add(new InvoiceSeries { Prefix = "GST", StartingNumber = 1, CurrentNumber = 0, FinancialYear = "2026-27", IsActive = true });
        if (!await db.HsnSacCodes.AnyAsync()) db.HsnSacCodes.AddRange(new HsnSacCode { Code = "8471", Type = "HSN", Description = "Computer hardware", GSTPercentage = 18, IsActive = true }, new HsnSacCode { Code = "9983", Type = "SAC", Description = "Professional services", GSTPercentage = 18, IsActive = true });
        if (!await db.CompanyProfiles.AnyAsync()) db.CompanyProfiles.Add(new CompanyProfile { CompanyName = "Advanced GST Demo Pvt Ltd", GSTIN = "27ABCDE1234F1Z5", PANNumber = "ABCDE1234F", Address = "Demo Business Park", State = "Maharashtra", City = "Mumbai", Pincode = "400001", Email = "accounts@example.com", PhoneNumber = "9999999999", BankName = "Demo Bank", AccountNumber = "000123456789", IFSCCode = "DEMO0001234", BranchName = "Mumbai", TermsAndConditions = "Goods once sold will not be returned.", DefaultInvoicePrefix = "GST", FinancialYear = "2026-27", IsActive = true });
        if (!await db.Customers.AnyAsync()) db.Customers.Add(new Customer { CustomerName = "Sample Customer", GSTIN = "27AAAAA0000A1Z5", PANNumber = "AAAAA0000A", BillingAddress = "Customer billing address", ShippingAddress = "Customer shipping address", State = "Maharashtra", City = "Pune", Pincode = "411001", Email = "customer@example.com", IsRegisteredGST = true, IsActive = true });
        if (!await db.Suppliers.AnyAsync()) db.Suppliers.Add(new Supplier { SupplierName = "Sample Supplier", GSTIN = "29BBBBB1111B1Z6", PANNumber = "BBBBB1111B", Address = "Supplier address", State = "Karnataka", City = "Bengaluru", Pincode = "560001", Email = "supplier@example.com", IsRegisteredGST = true, IsActive = true });
        if (!await db.Products.AnyAsync()) db.Products.Add(new Product { ProductName = "Laptop", ProductCode = "LAP-001", HSNCode = "8471", GSTPercentage = 18, Unit = "Nos", SaleRate = 65000, PurchaseRate = 55000, OpeningStock = 10, CurrentStock = 10, IsActive = true });
        if (!await db.ServiceItems.AnyAsync()) db.ServiceItems.Add(new ServiceItem { ServiceName = "Accounting Service", SACCode = "9983", GSTPercentage = 18, Rate = 5000, IsActive = true });
        if (!await db.AppSettings.AnyAsync()) db.AppSettings.AddRange(new AppSetting { Key = "EnableMockGstApi", Value = "true" }, new AppSetting { Key = "EnableEInvoice", Value = "true" }, new AppSetting { Key = "EnableEWayBill", Value = "true" });
        await db.SaveChangesAsync();
    }
}
