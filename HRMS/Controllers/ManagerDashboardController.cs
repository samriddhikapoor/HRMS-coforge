using HRMS.Data;
using HRMS.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HRMS.Controllers
{
    [Authorize(Roles = "Manager")]
    public class ManagerDashboardController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly HRMSContext _context;

        public ManagerDashboardController(
            UserManager<ApplicationUser> userManager,
            HRMSContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var currentUser =
                await _userManager.GetUserAsync(User);

            if (currentUser == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var manager = await _context.Managers
                .Include(m => m.Department)
                .FirstOrDefaultAsync(m => m.UserId == currentUser.Id);

            if (manager == null)
            {
                return NotFound("Manager profile not found.");
            }

            var employees = await _context.Employees
                .Include(e => e.Department)
                .Where(e => e.DepartmentId == manager.DepartmentId)
                .ToListAsync();

            ViewBag.ManagerName = currentUser.UserName;
            ViewBag.DepartmentName = manager.Department?.DepartmentName;
            ViewBag.TotalEmployees = employees.Count;

            return View(employees);
        }
    }
}