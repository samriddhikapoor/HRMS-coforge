using System.ComponentModel.DataAnnotations;

namespace HRMS.ViewModels.Manager
{
    public class ManagerViewModel
    {
        public int ManagerId { get; set; }

        [Required(ErrorMessage = "Manager Name is required")]
        [StringLength(100)]
        [Display(Name = "Manager Name")]
        public string ManagerName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Enter a valid email address")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Phone Number is required")]
        [Phone(ErrorMessage = "Enter a valid phone number")]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "Designation is required")]
        [StringLength(100)]
        public string Designation { get; set; } = "Manager";

        [Required(ErrorMessage = "Joining Date is required")]
        [DataType(DataType.Date)]
        [Display(Name = "Joining Date")]
        public DateTime JoiningDate { get; set; }

        [Required(ErrorMessage = "Please select a department")]
        [Display(Name = "Department")]
        public int DepartmentId { get; set; }

        // Used to display the selected department name
        public string? DepartmentName { get; set; }

        // Identity User
        public string? UserId { get; set; }
        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; } = string.Empty;
    }
}