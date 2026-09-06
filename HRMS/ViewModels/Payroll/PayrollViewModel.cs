using System.ComponentModel.DataAnnotations;

namespace HRMS.ViewModels.Payroll
{
    public class PayrollViewModel
    {
        public int PayrollId { get; set; }

        [Required(ErrorMessage = "Basic Salary is required")]
        [Range(0, double.MaxValue)]
        public decimal BasicSalary { get; set; }

        [Range(0, double.MaxValue)]
        public decimal Allowance { get; set; }

        [Range(0, double.MaxValue)]
        public decimal Deduction { get; set; }

        public decimal NetSalary { get; set; }

        [Required(ErrorMessage = "Salary Month is required")]
        public DateTime SalaryMonth { get; set; }

        [Required(ErrorMessage = "Please select an employee")]
        public int EmployeeId { get; set; }

        public string? EmployeeName { get; set; }
    }
}