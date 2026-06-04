using AdvancedGSTApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AdvancedGSTApp.Data;

public static class DbSeeder
{
    private const string SuperAdminRole = "Super Admin";
    private const string DefaultAdminEmail = "admin@gstsoftware.com";

    public static async Task SeedAsync(IServiceProvider services)
    {
        var db = services.GetRequiredService<ApplicationDbContext>();

        // Apply pending migrations before creating roles, users, and master data.
        await ApplyDatabaseChangesAsync(db);

        var roleManager = services.GetRequiredService<RoleManager<ApplicationRole>>();
        var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();

        await SeedRolesAsync(roleManager);
        await SeedSuperAdminAsync(userManager);
        await SeedMasterDataAsync(db);
    }

    private static async Task ApplyDatabaseChangesAsync(ApplicationDbContext db)
    {
        if (!db.Database.IsRelational())
        {
            await db.Database.EnsureCreatedAsync();
            return;
        }

        var migrations = db.Database.GetMigrations();

        if (migrations.Any())
        {
            await db.Database.MigrateAsync();
        }
        else
        {
            // This project may be run before an initial migration is created.
            // EnsureCreated keeps first-run development/demo databases usable.
            await db.Database.EnsureCreatedAsync();
        }
    }

    private static async Task SeedRolesAsync(RoleManager<ApplicationRole> roleManager)
    {
        // Default application roles used by authorization and the demo admin user.
        var roles = new[]
        {
            new ApplicationRole { Name = SuperAdminRole, Description = "Full access to all application modules and settings." },
            new ApplicationRole { Name = "Admin", Description = "Administrative access to master data and transactions." },
            new ApplicationRole { Name = "Accountant", Description = "Accounting, GST return, invoice, and report access." },
            new ApplicationRole { Name = "Staff", Description = "Limited day-to-day operational access." }
        };

        foreach (var role in roles)
        {
            if (await roleManager.RoleExistsAsync(role.Name!))
            {
                continue;
            }

            await ThrowIfFailedAsync(roleManager.CreateAsync(role), $"create role '{role.Name}'");
        }
    }

    private static async Task SeedSuperAdminAsync(UserManager<ApplicationUser> userManager)
    {
        // ASP.NET Core Identity hashes the password when CreateAsync is called.
        var admin = await userManager.FindByEmailAsync(DefaultAdminEmail);
        if (admin is null)
        {
            admin = new ApplicationUser
            {
                UserName = DefaultAdminEmail,
                Email = DefaultAdminEmail,
                EmailConfirmed = true,
                FullName = "Super Admin",
                IsActive = true
            };

            await ThrowIfFailedAsync(userManager.CreateAsync(admin, "Admin@123"), $"create user '{DefaultAdminEmail}'");
        }

        if (!await userManager.IsInRoleAsync(admin, SuperAdminRole))
        {
            await ThrowIfFailedAsync(userManager.AddToRoleAsync(admin, SuperAdminRole), $"assign '{SuperAdminRole}' role to '{DefaultAdminEmail}'");
        }
    }

    private static async Task SeedMasterDataAsync(ApplicationDbContext db)
    {
        await SeedCompanyProfileAsync(db);
        await SeedGstRatesAsync(db);
        await SeedInvoiceSeriesAsync(db);
        await SeedHsnSacCodesAsync(db);
        await SeedCustomersAsync(db);
        await SeedSuppliersAsync(db);
        await SeedProductsAsync(db);
        await SeedServicesAsync(db);
        await SeedAppSettingsAsync(db);
        await SeedGstApiCredentialAsync(db);

        await db.SaveChangesAsync();
    }

    private static async Task SeedCompanyProfileAsync(ApplicationDbContext db)
    {
        // Demo company details used for invoices, GST reports, and API credentials.
        if (await db.CompanyProfiles.AnyAsync(x => x.GSTIN == "24ABCDE1234F1Z5"))
        {
            return;
        }

        db.CompanyProfiles.Add(new CompanyProfile
        {
            CompanyName = "Demo GST Company",
            GSTIN = "24ABCDE1234F1Z5",
            PANNumber = "ABCDE1234F",
            Address = "101, Business Plaza, Ring Road",
            State = "Gujarat",
            City = "Surat",
            Pincode = "395003",
            PhoneNumber = "9876543210",
            Email = "info@demogstcompany.com",
            BankName = "Demo Bank",
            AccountNumber = "123456789012",
            IFSCCode = "DEMO0001234",
            BranchName = "Surat Main Branch",
            TermsAndConditions = "Goods once sold will not be returned.",
            DefaultInvoicePrefix = "GST",
            FinancialYear = "2026-27",
            IsActive = true
        });
    }

    private static async Task SeedGstRatesAsync(ApplicationDbContext db)
    {
        // Common GST slabs used by products, services, and invoice calculations.
        foreach (var rate in new[] { 0m, 5m, 12m, 18m, 28m })
        {
            if (await db.GstRates.AnyAsync(x => x.GSTPercentage == rate))
            {
                continue;
            }

            db.GstRates.Add(new GstRate
            {
                GSTPercentage = rate,
                Description = $"GST {rate:0}%",
                IsActive = true
            });
        }
    }

    private static async Task SeedInvoiceSeriesAsync(ApplicationDbContext db)
    {
        // CurrentNumber is seeded to 1 so the format example resolves to GST/2026-27/0001.
        if (await db.InvoiceSeries.AnyAsync(x => x.Prefix == "GST" && x.FinancialYear == "2026-27"))
        {
            return;
        }

        db.InvoiceSeries.Add(new InvoiceSeries
        {
            Prefix = "GST",
            FinancialYear = "2026-27",
            StartingNumber = 1,
            CurrentNumber = 1,
            IsActive = true
        });
    }

    private static async Task SeedHsnSacCodesAsync(ApplicationDbContext db)
    {
        // Standard HSN and SAC masters used to auto-fill GST percentages.
        var codes = new[]
        {
            new HsnSacCode { Code = "8517", Type = "HSN", Description = "Mobile Phones", GSTPercentage = 18, IsActive = true },
            new HsnSacCode { Code = "8471", Type = "HSN", Description = "Computers and Laptops", GSTPercentage = 18, IsActive = true },
            new HsnSacCode { Code = "8528", Type = "HSN", Description = "Monitors and Displays", GSTPercentage = 18, IsActive = true },
            new HsnSacCode { Code = "8504", Type = "HSN", Description = "Chargers and Adapters", GSTPercentage = 18, IsActive = true },
            new HsnSacCode { Code = "8544", Type = "HSN", Description = "Cables and Wires", GSTPercentage = 18, IsActive = true },
            new HsnSacCode { Code = "998313", Type = "SAC", Description = "IT Consulting Services", GSTPercentage = 18, IsActive = true },
            new HsnSacCode { Code = "998314", Type = "SAC", Description = "IT Design and Development Services", GSTPercentage = 18, IsActive = true },
            new HsnSacCode { Code = "998315", Type = "SAC", Description = "Hosting and IT Infrastructure Services", GSTPercentage = 18, IsActive = true },
            new HsnSacCode { Code = "998316", Type = "SAC", Description = "Software Support Services", GSTPercentage = 18, IsActive = true }
        };

        foreach (var code in codes)
        {
            if (await db.HsnSacCodes.AnyAsync(x => x.Code == code.Code && x.Type == code.Type))
            {
                continue;
            }

            db.HsnSacCodes.Add(code);
        }
    }

    private static async Task SeedCustomersAsync(ApplicationDbContext db)
    {
        var customers = new[]
        {
            new Customer { CustomerName = "Ramesh Traders", GSTIN = "24AABCR1234F1Z1", PANNumber = "AABCR1234F", PhoneNumber = "9876500001", Email = "ramesh@example.com", BillingAddress = "Surat, Gujarat", ShippingAddress = "Surat, Gujarat", State = "Gujarat", City = "Surat", Pincode = "395006", IsRegisteredGST = true, IsActive = true },
            new Customer { CustomerName = "Maharashtra Retail Pvt Ltd", GSTIN = "27AAECM1234F1Z2", PANNumber = "AAECM1234F", PhoneNumber = "9876500002", Email = "maharashtra@example.com", BillingAddress = "Mumbai, Maharashtra", ShippingAddress = "Mumbai, Maharashtra", State = "Maharashtra", City = "Mumbai", Pincode = "400001", IsRegisteredGST = true, IsActive = true }
        };

        foreach (var customer in customers)
        {
            if (await db.Customers.AnyAsync(x => x.GSTIN == customer.GSTIN || x.Email == customer.Email))
            {
                continue;
            }

            db.Customers.Add(customer);
        }
    }

    private static async Task SeedSuppliersAsync(ApplicationDbContext db)
    {
        var suppliers = new[]
        {
            new Supplier { SupplierName = "Gujarat Mobile Distributor", GSTIN = "24AABCG5678K1Z3", PANNumber = "AABCG5678K", PhoneNumber = "9876500011", Email = "suppliergujarat@example.com", Address = "Ahmedabad, Gujarat", State = "Gujarat", City = "Ahmedabad", Pincode = "380001", IsRegisteredGST = true, IsActive = true },
            new Supplier { SupplierName = "Delhi Electronics Supplier", GSTIN = "07AABCD6789L1Z4", PANNumber = "AABCD6789L", PhoneNumber = "9876500012", Email = "supplierdelhi@example.com", Address = "New Delhi, Delhi", State = "Delhi", City = "New Delhi", Pincode = "110001", IsRegisteredGST = true, IsActive = true }
        };

        foreach (var supplier in suppliers)
        {
            if (await db.Suppliers.AnyAsync(x => x.GSTIN == supplier.GSTIN || x.Email == supplier.Email))
            {
                continue;
            }

            db.Suppliers.Add(supplier);
        }
    }

    private static async Task SeedProductsAsync(ApplicationDbContext db)
    {
        var products = new[]
        {
            new Product { ProductName = "iPhone 14", ProductCode = "IPH14", HSNCode = "8517", GSTPercentage = 18, Unit = "Nos", SaleRate = 55000, PurchaseRate = 50000, OpeningStock = 10, CurrentStock = 10, IsActive = true },
            new Product { ProductName = "Samsung Galaxy S23", ProductCode = "SGS23", HSNCode = "8517", GSTPercentage = 18, Unit = "Nos", SaleRate = 45000, PurchaseRate = 41000, OpeningStock = 15, CurrentStock = 15, IsActive = true },
            new Product { ProductName = "Laptop Charger", ProductCode = "CHG001", HSNCode = "8504", GSTPercentage = 18, Unit = "Nos", SaleRate = 1200, PurchaseRate = 800, OpeningStock = 50, CurrentStock = 50, IsActive = true }
        };

        foreach (var product in products)
        {
            if (await db.Products.AnyAsync(x => x.ProductCode == product.ProductCode))
            {
                continue;
            }

            db.Products.Add(product);
        }
    }

    private static async Task SeedServicesAsync(ApplicationDbContext db)
    {
        var services = new[]
        {
            new ServiceItem { ServiceName = "Software Development Service", SACCode = "998314", GSTPercentage = 18, Rate = 25000, IsActive = true },
            new ServiceItem { ServiceName = "IT Support Service", SACCode = "998316", GSTPercentage = 18, Rate = 5000, IsActive = true }
        };

        foreach (var service in services)
        {
            if (await db.ServiceItems.AnyAsync(x => x.ServiceName == service.ServiceName && x.SACCode == service.SACCode))
            {
                continue;
            }

            db.ServiceItems.Add(service);
        }
    }

    private static async Task SeedAppSettingsAsync(ApplicationDbContext db)
    {
        // App settings keep demo defaults and list values in a simple, editable form.
        var settings = new[]
        {
            new AppSetting { Key = "FinancialYear", Value = "2026-27", Description = "Default financial year for transactions and GST returns." },
            new AppSetting { Key = "DefaultGstRate", Value = "18", Description = "Default GST percentage used by new taxable items." },
            new AppSetting { Key = "DefaultState", Value = "Gujarat", Description = "Default place of supply/state for the company." },
            new AppSetting { Key = "EnableMockGstApi", Value = "true", Description = "Use mock GST API responses for sandbox/demo mode." },
            new AppSetting { Key = "EnableEInvoice", Value = "true", Description = "Enable e-invoice workflow in the application." },
            new AppSetting { Key = "EnableEWayBill", Value = "true", Description = "Enable e-way bill workflow in the application." },
            new AppSetting { Key = "SmtpEnabled", Value = "false", Description = "SMTP email sending toggle." },
            new AppSetting { Key = "ExpenseCategories", Value = "Office Rent,Internet Bill,Electricity Bill,Office Supplies,Transport,Staff Salary,Software Subscription,Miscellaneous", Description = "Comma-separated default expense categories." },
            new AppSetting { Key = "PaymentModes", Value = "Cash,Bank Transfer,UPI,Cheque,Card", Description = "Comma-separated default payment modes." }
        };

        foreach (var setting in settings)
        {
            if (await db.AppSettings.AnyAsync(x => x.Key == setting.Key))
            {
                continue;
            }

            db.AppSettings.Add(setting);
        }
    }

    private static async Task SeedGstApiCredentialAsync(ApplicationDbContext db)
    {
        // Sandbox credential used by the mock GST API integration.
        if (await db.GstApiCredentials.AnyAsync(x => x.ProviderName == "Demo GST API Provider" && x.Environment == "Sandbox" && x.GSTIN == "24ABCDE1234F1Z5"))
        {
            return;
        }

        db.GstApiCredentials.Add(new GstApiCredential
        {
            ProviderName = "Demo GST API Provider",
            BaseUrl = "https://demo-gst-api.local",
            ClientId = "demo-client-id",
            ClientSecret = "demo-client-secret",
            Username = "demo-user",
            Password = "demo-password",
            GSTIN = "24ABCDE1234F1Z5",
            Environment = "Sandbox",
            IsActive = true
        });
    }

    private static async Task ThrowIfFailedAsync(Task<IdentityResult> identityTask, string action)
    {
        var result = await identityTask;
        if (result.Succeeded)
        {
            return;
        }

        var errors = string.Join(", ", result.Errors.Select(x => x.Description));
        throw new InvalidOperationException($"Failed to {action}: {errors}");
    }
}
