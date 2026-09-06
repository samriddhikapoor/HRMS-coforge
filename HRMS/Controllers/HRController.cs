using HRMS.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HRMS.Controllers
{
    [Authorize(Roles = "Admin")]
    public class HRController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public HRController(
            UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }


        // =========================================================
        // HR LIST
        // =========================================================

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var hrUsers =
                await _userManager.GetUsersInRoleAsync("HR");

            return View(hrUsers);
        }


        // =========================================================
        // HR DETAILS
        // =========================================================

        [HttpGet]
        public async Task<IActionResult> Details(string id)
        {
            if (string.IsNullOrEmpty(id))
                return NotFound();

            var user =
                await _userManager.FindByIdAsync(id);

            if (user == null)
                return NotFound();

            var roles =
                await _userManager.GetRolesAsync(user);

            if (!roles.Contains("HR"))
                return NotFound();

            return View(user);
        }


        // =========================================================
        // EDIT HR - GET
        // =========================================================

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            if (string.IsNullOrEmpty(id))
                return NotFound();

            var user =
                await _userManager.FindByIdAsync(id);

            if (user == null)
                return NotFound();

            var roles =
                await _userManager.GetRolesAsync(user);

            if (!roles.Contains("HR"))
                return NotFound();

            return View(user);
        }


        // =========================================================
        // EDIT HR - POST
        // =========================================================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            string id,
            ApplicationUser model)
        {
            if (string.IsNullOrEmpty(id))
                return NotFound();

            var user =
                await _userManager.FindByIdAsync(id);

            if (user == null)
                return NotFound();

            var roles =
                await _userManager.GetRolesAsync(user);

            if (!roles.Contains("HR"))
                return NotFound();


            // -----------------------------
            // CHECK EMAIL
            // -----------------------------

            if (user.Email != model.Email)
            {
                var existingUser =
                    await _userManager.FindByEmailAsync(model.Email);

                if (existingUser != null &&
                    existingUser.Id != user.Id)
                {
                    ModelState.AddModelError(
                        "Email",
                        "This email is already in use.");

                    return View(model);
                }

                user.Email = model.Email;
                user.NormalizedEmail =
                    model.Email.ToUpper();
            }


            // -----------------------------
            // UPDATE USERNAME
            // -----------------------------

            if (user.UserName != model.UserName)
            {
                var existingUsername =
                    await _userManager.FindByNameAsync(model.UserName);

                if (existingUsername != null &&
                    existingUsername.Id != user.Id)
                {
                    ModelState.AddModelError(
                        "UserName",
                        "This username is already in use.");

                    return View(model);
                }

                user.UserName = model.UserName;
                user.NormalizedUserName =
                    model.UserName.ToUpper();
            }


            // -----------------------------
            // SAVE
            // -----------------------------

            var result =
                await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(
                        string.Empty,
                        error.Description);
                }

                return View(model);
            }


            TempData["SuccessMessage"] =
                "HR details updated successfully.";

            return RedirectToAction(nameof(Index));
        }


        // =========================================================
        // DEACTIVATE HR - GET
        // =========================================================

        [HttpGet]
        public async Task<IActionResult> Deactivate(string id)
        {
            if (string.IsNullOrEmpty(id))
                return NotFound();

            var user =
                await _userManager.FindByIdAsync(id);

            if (user == null)
                return NotFound();

            var roles =
                await _userManager.GetRolesAsync(user);

            if (!roles.Contains("HR"))
                return NotFound();

            return View(user);
        }


        // =========================================================
        // DEACTIVATE HR - POST
        // =========================================================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeactivateConfirmed(
            string id)
        {
            if (string.IsNullOrEmpty(id))
                return NotFound();

            var user =
                await _userManager.FindByIdAsync(id);

            if (user == null)
                return NotFound();

            var roles =
                await _userManager.GetRolesAsync(user);

            if (!roles.Contains("HR"))
                return NotFound();


            // Enable lockout and deactivate account
            user.LockoutEnabled = true;
            user.LockoutEnd =
                DateTimeOffset.UtcNow.AddYears(100);

            var result =
                await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(
                        string.Empty,
                        error.Description);
                }

                return View("Deactivate", user);
            }


            TempData["SuccessMessage"] =
                "HR account deactivated successfully.";

            return RedirectToAction(nameof(Index));
        }


        // =========================================================
        // ACTIVATE HR
        // =========================================================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Activate(string id)
        {
            if (string.IsNullOrEmpty(id))
                return NotFound();

            var user =
                await _userManager.FindByIdAsync(id);

            if (user == null)
                return NotFound();

            var roles =
                await _userManager.GetRolesAsync(user);

            if (!roles.Contains("HR"))
                return NotFound();


            user.LockoutEnd = null;

            var result =
                await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(
                        string.Empty,
                        error.Description);
                }

                return RedirectToAction(nameof(Index));
            }


            TempData["SuccessMessage"] =
                "HR account activated successfully.";

            return RedirectToAction(nameof(Index));
        }
    }
}