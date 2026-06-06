using AdvancedGSTApp.Filters;
using AdvancedGSTApp.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AdvancedGSTApp.Controllers;

[Authorize(Roles = "Super Admin,Admin,Accountant")]
[PermissionAuthorize("Reports", "View")]
public class ReportsController(IGstReportService reports) : Controller
{
    public async Task<IActionResult> SalesRegister(int? month, string? financialYear) => View(await reports.GetGstr1Async(month, financialYear));
}
