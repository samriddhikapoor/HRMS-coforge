using AutoMapper;
using HRMS.Data;
using HRMS.Models;
using HRMS.ViewModels.Manager;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace HRMS.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ManagerController : Controller
    {
        private readonly HRMSContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _mapper;

        public ManagerController(
            HRMSContext context,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IMapper mapper)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _mapper = mapper;
        }


        // =========================================================
        // MANAGER LIST
        // =========================================================

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var managers = await _context.Managers
                .Include(m => m.Department)
                .OrderBy(m => m.ManagerName)
                .ToListAsync();

            return View(managers);
        }


        // =========================================================
        // CREATE MANAGER - GET
        // =========================================================

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            await LoadDepartments();

            var model = new ManagerViewModel
            {
                JoiningDate = DateTime.Today,
                Designation = "Manager"
            };

            return View(model);
        }


        // =========================================================
        // CREATE MANAGER - POST
        // =========================================================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ManagerViewModel model)
        {
            if (!ModelState.IsValid)
            {
                await LoadDepartments();
                return View(model);
            }


            // Check duplicate Identity account
            var existingUser =
                await _userManager.FindByEmailAsync(model.Email);

            if (existingUser != null)
            {
                ModelState.AddModelError(
                    "Email",
                    "An account with this email already exists.");

                await LoadDepartments();
                return View(model);
            }


            // Check duplicate Manager
            var existingManager =
                await _context.Managers
                    .FirstOrDefaultAsync(m => m.Email == model.Email);

            if (existingManager != null)
            {
                ModelState.AddModelError(
                    "Email",
                    "A manager with this email already exists.");

                await LoadDepartments();
                return View(model);
            }


            // Create Identity User
            var user = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email,
                EmailConfirmed = true
            };

            var userResult =
                await _userManager.CreateAsync(
                    user,
                    model.Password);

            if (!userResult.Succeeded)
            {
                foreach (var error in userResult.Errors)
                {
                    ModelState.AddModelError(
                        string.Empty,
                        error.Description);
                }

                await LoadDepartments();
                return View(model);
            }


            // Make sure Manager role exists
            if (!await _roleManager.RoleExistsAsync("Manager"))
            {
                await _userManager.DeleteAsync(user);

                ModelState.AddModelError(
                    string.Empty,
                    "Manager role does not exist.");

                await LoadDepartments();
                return View(model);
            }


            // Assign Manager role
            var roleResult =
                await _userManager.AddToRoleAsync(
                    user,
                    "Manager");

            if (!roleResult.Succeeded)
            {
                await _userManager.DeleteAsync(user);

                foreach (var error in roleResult.Errors)
                {
                    ModelState.AddModelError(
                        string.Empty,
                        error.Description);
                }

                await LoadDepartments();
                return View(model);
            }


            // Create Manager profile
            var manager = _mapper.Map<Manager>(model);

            manager.UserId = user.Id;
            manager.Designation = "Manager";

            _context.Managers.Add(manager);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch
            {
                // Rollback Identity account
                await _userManager.RemoveFromRoleAsync(
                    user,
                    "Manager");

                await _userManager.DeleteAsync(user);

                ModelState.AddModelError(
                    string.Empty,
                    "Manager could not be created. Please try again.");

                await LoadDepartments();
                return View(model);
            }


            TempData["SuccessMessage"] =
                $"Manager {model.ManagerName} created successfully.";

            return RedirectToAction(nameof(Index));
        }


        // =========================================================
        // MANAGER DETAILS
        // =========================================================

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var manager = await _context.Managers
                .Include(m => m.Department)
                .FirstOrDefaultAsync(m => m.ManagerId == id);

            if (manager == null)
            {
                return NotFound();
            }

            return View(manager);
        }


        // =========================================================
        // EDIT MANAGER - GET
        // =========================================================

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var manager = await _context.Managers
                .FirstOrDefaultAsync(m => m.ManagerId == id);

            if (manager == null)
            {
                return NotFound();
            }

            var model =
                _mapper.Map<ManagerViewModel>(manager);

            await LoadDepartments();

            return View(model);
        }


        // =========================================================
        // EDIT MANAGER - POST
        // =========================================================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id,
            ManagerViewModel model)
        {
            if (id != model.ManagerId)
            {
                return NotFound();
            }


            if (!ModelState.IsValid)
            {
                await LoadDepartments();
                return View(model);
            }


            var manager = await _context.Managers
                .FirstOrDefaultAsync(m => m.ManagerId == id);

            if (manager == null)
            {
                return NotFound();
            }


            // Check duplicate email
            var emailExists =
                await _context.Managers
                    .AnyAsync(m =>
                        m.Email == model.Email &&
                        m.ManagerId != id);

            if (emailExists)
            {
                ModelState.AddModelError(
                    "Email",
                    "Another manager already uses this email.");

                await LoadDepartments();
                return View(model);
            }


            // Update Manager profile
            _mapper.Map(model, manager);

            manager.Designation = "Manager";

            await _context.SaveChangesAsync();


            // Update Identity email also
            if (!string.IsNullOrEmpty(manager.UserId))
            {
                var user =
                    await _userManager.FindByIdAsync(manager.UserId);

                if (user != null && user.Email != model.Email)
                {
                    user.Email = model.Email;
                    user.UserName = model.Email;

                    await _userManager.UpdateAsync(user);
                }
            }


            TempData["SuccessMessage"] =
                "Manager updated successfully.";

            return RedirectToAction(nameof(Index));
        }


        // =========================================================
        // DELETE MANAGER - GET
        // =========================================================

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var manager = await _context.Managers
                .Include(m => m.Department)
                .FirstOrDefaultAsync(m => m.ManagerId == id);

            if (manager == null)
            {
                return NotFound();
            }

            return View(manager);
        }


        // =========================================================
        // DELETE MANAGER - POST
        // =========================================================

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var manager = await _context.Managers
                .FirstOrDefaultAsync(m => m.ManagerId == id);

            if (manager == null)
            {
                return NotFound();
            }


            // Delete linked Identity account
            if (!string.IsNullOrEmpty(manager.UserId))
            {
                var user =
                    await _userManager.FindByIdAsync(manager.UserId);

                if (user != null)
                {
                    await _userManager.DeleteAsync(user);
                }
            }


            _context.Managers.Remove(manager);

            await _context.SaveChangesAsync();


            TempData["SuccessMessage"] =
                "Manager deleted successfully.";

            return RedirectToAction(nameof(Index));
        }


        // =========================================================
        // LOAD DEPARTMENTS
        // =========================================================

        private async Task LoadDepartments()
        {
            ViewBag.Departments =
                await _context.Departments
                    .OrderBy(d => d.DepartmentName)
                    .Select(d => new SelectListItem
                    {
                        Value = d.DepartmentId.ToString(),
                        Text = d.DepartmentName
                    })
                    .ToListAsync();
        }
    }
}