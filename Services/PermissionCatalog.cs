namespace AdvancedGSTApp.Services;

public static class PermissionCatalog
{
    public const string SuperAdminRole = "Super Admin";

    public static readonly string[] Modules =
    [
        "Dashboard", "Company Profile", "Customers", "Suppliers", "Products", "Services", "HSN/SAC Codes", "GST Rates",
        "Invoice Series", "Sales Invoice", "Purchase Invoice", "Credit Note", "Debit Note", "Expenses", "Receipts", "Payments",
        "GST Payable Summary", "Input Tax Credit", "Output Tax", "GST Challan", "GST Payment Tracking", "GSTR-1 Report",
        "GSTR-3B Report", "GST Return Filing", "GST Return Status", "GST Ledger", "GST Reconciliation", "E-Invoice", "E-Way Bill",
        "Sales Register", "Purchase Register", "Expense Register", "Customer Statement", "Supplier Statement", "Reports", "Settings",
        "User Management", "Role Management", "Audit Logs", "GST API Credentials", "GST API Logs"
    ];

    public static readonly string[] PermissionNames = ["View", "Create", "Edit", "Delete", "Print", "Export", "Email", "Generate", "Approve"];
}
