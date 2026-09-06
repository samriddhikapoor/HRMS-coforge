using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HRMS.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            if (User.IsInRole("HR"))
            {
                return RedirectToAction(
                    "Index",
                    "HRDashboard");
            }

            if (User.IsInRole("Employee"))
            {
                return RedirectToAction(
                    "Index",
                    "EmployeeDashboard");
            }

            return Forbid();
        }
    }
}