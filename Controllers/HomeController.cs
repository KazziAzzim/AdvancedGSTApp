using AdvancedGSTApp.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AdvancedGSTApp.Controllers;
[Authorize]
public class HomeController(IGstReportService reports) : Controller
{
    public async Task<IActionResult> Index() => View(await reports.GetDashboardAsync());
    [AllowAnonymous] public IActionResult Error() => View();
}
