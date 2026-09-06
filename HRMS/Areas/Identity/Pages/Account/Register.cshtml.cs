using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using HRMS.Data;
using HRMS.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace HRMS.Areas.Identity.Pages.Account;

public class RegisterModel : PageModel
{
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly HRMSContext _context;
    private readonly ILogger<RegisterModel> _logger;

    public RegisterModel(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        HRMSContext context,
        ILogger<RegisterModel> logger)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _context = context;
        _logger = logger;
    }

    [BindProperty]
    public InputModel Input { get; set; } = new();

    public string? ReturnUrl { get; set; }

    public IList<AuthenticationScheme>? ExternalLogins { get; set; }

    public List<Department> Departments { get; set; } = new();

    public class InputModel
    {
        [Required(ErrorMessage = "Employee name is required.")]
        [StringLength(100)]
        [Display(Name = "Employee Name")]
        public string EmployeeName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Enter a valid email address.")]
        [Display(Name = "Email")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Phone number is required.")]
        [Phone(ErrorMessage = "Enter a valid phone number.")]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "Designation is required.")]
        [StringLength(100)]
        [Display(Name = "Designation")]
        public string Designation { get; set; } = string.Empty;

        [Required(ErrorMessage = "Joining date is required.")]
        [Display(Name = "Joining Date")]
        public DateTime JoiningDate { get; set; } = DateTime.Today;

        [Required(ErrorMessage = "Please select a department.")]
        [Display(Name = "Department")]
        public int DepartmentId { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [StringLength(
            100,
            ErrorMessage = "Password must be at least {2} characters long.",
            MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please confirm your password.")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Compare(
            "Password",
            ErrorMessage = "The password and confirmation password do not match.")]
        public string? ConfirmPassword { get; set; }
    }

    public async Task OnGetAsync(string? returnUrl = null)
    {
        ReturnUrl = returnUrl;

        ExternalLogins =
            (await _signInManager
                .GetExternalAuthenticationSchemesAsync())
            .ToList();

        await LoadDepartmentsAsync();
    }

    public async Task<IActionResult> OnPostAsync(string? returnUrl = null)
    {
        returnUrl ??= Url.Content("~/");

        ReturnUrl = returnUrl;

        ExternalLogins =
            (await _signInManager
                .GetExternalAuthenticationSchemesAsync())
            .ToList();

        await LoadDepartmentsAsync();

        if (!ModelState.IsValid)
        {
            return Page();
        }

        // Check whether email already exists
        var existingUser =
            await _userManager.FindByEmailAsync(Input.Email);

        if (existingUser != null)
        {
            ModelState.AddModelError(
                "Input.Email",
                "An account with this email already exists.");

            return Page();
        }

        // Check department
        var department =
            await _context.Departments
                .FirstOrDefaultAsync(
                    d => d.DepartmentId == Input.DepartmentId);

        if (department == null)
        {
            ModelState.AddModelError(
                "Input.DepartmentId",
                "Please select a valid department.");

            return Page();
        }

        // =========================
        // CREATE APPLICATION USER
        // =========================

        var user = new ApplicationUser
        {
            UserName = Input.Email,
            Email = Input.Email
        };

        var userResult =
            await _userManager.CreateAsync(
                user,
                Input.Password);

        if (!userResult.Succeeded)
        {
            foreach (var error in userResult.Errors)
            {
                ModelState.AddModelError(
                    string.Empty,
                    error.Description);
            }

            return Page();
        }

        // =========================
        // ASSIGN EMPLOYEE ROLE
        // =========================

        var roleResult =
            await _userManager.AddToRoleAsync(
                user,
                "Employee");

        if (!roleResult.Succeeded)
        {
            await _userManager.DeleteAsync(user);

            foreach (var error in roleResult.Errors)
            {
                ModelState.AddModelError(
                    string.Empty,
                    error.Description);
            }

            return Page();
        }

        // =========================
        // CREATE EMPLOYEE RECORD
        // =========================

        var employee = new Employee
        {
            EmployeeName = Input.EmployeeName,
            Email = Input.Email,
            PhoneNumber = Input.PhoneNumber,
            Designation = Input.Designation,
            JoiningDate = Input.JoiningDate,
            DepartmentId = Input.DepartmentId,

            // IMPORTANT:
            // Link Employee with ApplicationUser
            UserId = user.Id
        };

        _context.Employees.Add(employee);

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (Exception)
        {
            // If employee creation fails,
            // remove the Identity user as well.
            await _userManager.DeleteAsync(user);

            ModelState.AddModelError(
                string.Empty,
                "Unable to create employee profile. Please try again.");

            return Page();
        }

        _logger.LogInformation(
            "Employee account and employee profile created successfully.");

        // =========================
        // AUTO LOGIN
        // =========================

        await _signInManager.SignInAsync(
            user,
            isPersistent: false);

        _logger.LogInformation(
            "New employee automatically signed in.");

        // =========================
        // EMPLOYEE DASHBOARD
        // =========================

        return RedirectToAction(
            "Index",
            "EmployeeDashboard");
    }

    private async Task LoadDepartmentsAsync()
    {
        Departments =
            await _context.Departments
                .OrderBy(d => d.DepartmentName)
                .ToListAsync();
    }
}