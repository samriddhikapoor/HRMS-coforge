using HRMS.Data;
using HRMS.ViewModels.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HRMS.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly HRMSContext _context;

        public AdminController(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            HRMSContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
        }


        // =========================================================
        // ADMIN DASHBOARD
        // =========================================================

        [HttpGet]
        public async Task<IActionResult> Dashboard()
        {
            // -----------------------------
            // SUMMARY COUNTS
            // -----------------------------

            ViewBag.TotalEmployees =
                await _context.Employees.CountAsync();

            ViewBag.TotalDepartments =
                await _context.Departments.CountAsync();

            ViewBag.TotalManagers =
                await _context.Managers.CountAsync();

            ViewBag.TotalSystemUsers =
                await _userManager.Users.CountAsync();

            ViewBag.PendingRequests =
                await _context.LeaveRequests
                    .CountAsync(l => l.Status == "Pending");


            // -----------------------------
            // ROLE COUNTS
            // -----------------------------

            var hrUsers =
                await _userManager.GetUsersInRoleAsync("HR");

            var managerUsers =
                await _userManager.GetUsersInRoleAsync("Manager");

            var employeeUsers =
                await _userManager.GetUsersInRoleAsync("Employee");

            ViewBag.HrCount = hrUsers.Count;
            ViewBag.ManagerCount = managerUsers.Count;
            ViewBag.EmployeeCount = employeeUsers.Count;


            // -----------------------------
            // RECENT USERS
            // -----------------------------

            var users = await _userManager.Users
                .OrderByDescending(u => u.Id)
                .Take(5)
                .ToListAsync();

            var recentUsers = new List<object>();

            foreach (var user in users)
            {
                var roles =
                    await _userManager.GetRolesAsync(user);

                var role =
                    roles.FirstOrDefault() ?? "User";


                // Try to find employee profile
                var employee =
                    await _context.Employees
                        .FirstOrDefaultAsync(
                            e => e.UserId == user.Id);


                // Try to find manager profile
                var manager =
                    await _context.Managers
                        .FirstOrDefaultAsync(
                            m => m.UserId == user.Id);


                var name =
                    employee?.EmployeeName
                    ?? manager?.ManagerName
                    ?? user.Email
                    ?? "User";


                recentUsers.Add(new
                {
                    Name = name,
                    Email = user.Email ?? "",
                    Role = role,

                    IsActive =
                        !user.LockoutEnd.HasValue ||
                        user.LockoutEnd <= DateTimeOffset.UtcNow
                });
            }


            ViewBag.RecentUsers = recentUsers;


            return View();
        }


        // =========================================================
        // CREATE USER
        // =========================================================

        [HttpGet]
        public async Task<IActionResult> CreateUser()
        {
            ViewBag.Roles =
                await _roleManager
                    .Roles
                    .Select(r => r.Name!)
                    .ToListAsync();

            return View();
        }


        // =========================================================
        // CREATE USER - POST
        // =========================================================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateUser(
            CreateUserViewModel model)
        {
            if (!ModelState.IsValid)
            {
                await LoadRoles();
                return View(model);
            }


            // -----------------------------
            // CHECK EXISTING USER
            // -----------------------------

            var existingUser =
                await _userManager
                    .FindByEmailAsync(model.Email);

            if (existingUser != null)
            {
                ModelState.AddModelError(
                    "Email",
                    "An account with this email already exists.");

                await LoadRoles();
                return View(model);
            }


            // -----------------------------
            // CHECK ROLE
            // -----------------------------

            if (!await _roleManager
                .RoleExistsAsync(model.Role))
            {
                ModelState.AddModelError(
                    "Role",
                    "Selected role does not exist.");

                await LoadRoles();
                return View(model);
            }


            // -----------------------------
            // CREATE IDENTITY USER
            // -----------------------------

            var user = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email,
                EmailConfirmed = true
            };


            var result =
                await _userManager.CreateAsync(
                    user,
                    model.Password);


            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(
                        string.Empty,
                        error.Description);
                }

                await LoadRoles();
                return View(model);
            }


            // -----------------------------
            // ASSIGN ROLE
            // -----------------------------

            var roleResult =
                await _userManager.AddToRoleAsync(
                    user,
                    model.Role);


            if (!roleResult.Succeeded)
            {
                await _userManager.DeleteAsync(user);

                foreach (var error in roleResult.Errors)
                {
                    ModelState.AddModelError(
                        string.Empty,
                        error.Description);
                }

                await LoadRoles();
                return View(model);
            }


            TempData["SuccessMessage"] =
                $"User {model.Email} created successfully as {model.Role}.";

            return RedirectToAction(nameof(Dashboard));
        }


        // =========================================================
        // LOAD ROLES
        // =========================================================

        private async Task LoadRoles()
        {
            ViewBag.Roles =
                await _roleManager
                    .Roles
                    .Select(r => r.Name!)
                    .ToListAsync();
        }
    }
}