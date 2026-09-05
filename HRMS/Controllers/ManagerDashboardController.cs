using HRMS.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HRMS.Controllers
{
    [Authorize(Roles = "Manager")]
    public class ManagerDashboardController : Controller
    {
        private readonly IEmployeeRepository _employeeRepository;

        public ManagerDashboardController(
            IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        public async Task<IActionResult> Index()
        {
            var userId =
                User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
            {
                return Unauthorized();
            }

            var employee =
                await _employeeRepository
                    .GetEmployeeByUserIdAsync(userId);

            if (employee == null)
            {
                return NotFound("Manager employee record not found.");
            }

            return View(employee);
        }
    }
}