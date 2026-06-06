# Advanced GST Billing & Compliance Software

An ASP.NET Core MVC, Razor Views, Bootstrap 5, jQuery and SQL Server GST billing/compliance application scaffolded for end-to-end operations and future GST Suvidha Provider / ASP-GSP integration.

> Current target framework: **.NET 10 / ASP.NET Core 10**, selected because Microsoft lists .NET 10 as in-support from 2025-11-11 through 2028-11-14 and ASP.NET Core follows the .NET lifecycle.

## Implemented Modules

- Authentication with ASP.NET Core Identity, seeded roles and seeded Super Admin.
- Admin dashboard with sales, purchase, ITC, GST return and e-invoice/e-way-bill status widgets.
- Company, customer, supplier, product, service, HSN/SAC, GST rate, invoice series entities.
- Sales invoice with Razor UI, dynamic jQuery rows, same-state CGST/SGST and interstate IGST calculation.
- Purchase invoices and ITC ledger posting.
- GST payable, GSTR-1 and GSTR-3B preparation views.
- GST return filing, challan generation and status tracking service flow.
- E-invoice, IRN, QR payload and e-way bill mock generation.
- Mock GST API service with request/response logging, retry attempts, failure simulation and manual fallback flag.
- Real GST API service placeholder for later ASP/GSP SDK integration.
- Ledgers, reconciliation, API credential, audit log, challan, payment and filing entities.
- Export/PDF/email service placeholders with interfaces for production implementations.

## Technology Stack

- ASP.NET Core MVC / Razor Views (`net10.0`)
- Entity Framework Core Code First
- SQL Server / LocalDB default connection
- ASP.NET Core Identity roles: `Super Admin`, `Admin`, `Accountant`, `Staff`
- Bootstrap 5, jQuery, jQuery Validate
- Repository pattern + service layer + ViewModels

## Setup

1. Install the latest .NET 10 SDK and SQL Server / SQL Server LocalDB.
2. Update `appsettings.json` connection string if needed:

```json
"DefaultConnection": "Server=(localdb)\\MSSQLLocalDB;Database=AdvancedGSTAppDb;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True"
```

3. Restore and run:

```bash
dotnet restore
dotnet run
```

4. Open the URL shown by `dotnet run` and sign in.

## Sample Login

- Email: `admin@advancedgst.local`
- Password: `Admin@12345`

Change this password immediately in a real environment.

## Database and Migrations

The app is Code First. A baseline migration marker is included under `Migrations/`. For a production-grade migration generated from the installed SDK, run:

```bash
dotnet tool install --global dotnet-ef
dotnet ef migrations add InitialCreate
dotnet ef database update
```

For demo readiness the seeder uses `EnsureCreatedAsync()` to create the schema from the EF model when the app starts. Switch to `MigrateAsync()` once real migrations are generated in your environment.

## Seed Data

The startup seeder creates:

- Default roles
- Default Super Admin user
- GST rates: 0%, 5%, 12%, 18%, 28%
- Invoice series: `GST/2026-27/0001`
- Sample HSN/SAC codes (`8471`, `9983`)
- Demo company, customer, supplier, product and service
- Default app settings for mock GST API, e-invoice and e-way bill enablement

## GST API Integration Architecture

`IGstApiService` defines all portal/GSP-ready operations:

- GSTIN, PAN and HSN/SAC validation
- E-invoice generation/cancellation
- E-way bill generation/cancellation
- Challan generation
- GSTR-1/GSTR-3B filing
- Return and payment status checks

`MockGstApiService` is the default and logs every request and response to `GstApiRequestLog` and `GstApiResponseLog`. It supports retries, simulated failures and a manual fallback flag. `RealGstApiService` is intentionally a placeholder to avoid hardcoded credentials and to keep production credentials out of source control.

Configure mock behavior in `appsettings.json`:

```json
"GstApi": {
  "Provider": "MockGSP",
  "BaseUrl": "https://sandbox.gst.example.local",
  "Environment": "Sandbox",
  "UseMock": true,
  "MaxRetries": 3,
  "FailureRatePercent": 15
}
```

## Sample Mock Responses

- GSTIN validation: active taxpayer payload with legal name and GSTIN.
- E-invoice: fake IRN, acknowledgement number and QR data payload.
- E-way bill: fake 12-digit EWB number.
- Challan: fake CPIN number.
- GST returns: fake ARN acknowledgement number.

## Security Notes

- All app pages require authentication except login and error pages.
- Accounting/GST actions are role-gated for Super Admin/Admin/Accountant where applicable.
- Forms use anti-forgery tokens.
- API credentials are represented as entities/settings, but secret values are not displayed in the provided UI and should be encrypted before production use.
- Server-side validation attributes are included on master entities and login forms; jQuery unobtrusive validation is loaded for client-side validation.

## Production Hardening Checklist

- Generate real EF migrations and use `MigrateAsync()`.
- Add encrypted secret storage for GSP credentials.
- Replace `PdfService` placeholder with a PDF renderer such as QuestPDF or wkhtmltopdf.
- Replace `EmailService` placeholder with SMTP delivery and email history persistence.
- Add complete CRUD views for every advanced entity based on the existing service/repository pattern.
- Add Excel/PDF exports using ClosedXML and a PDF implementation.
- Add integration tests around GST calculation, invoice posting and mock API retry behavior.
