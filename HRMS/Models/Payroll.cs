using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HRMS.Models
{
    public class Payroll
    {
        [Key]
        public int PayrollId { get; set; }

        [Required]
        public decimal BasicSalary { get; set; }

        public decimal Allowance { get; set; }

        public decimal Deduction { get; set; }

        [Required]
        public decimal NetSalary { get; set; }

        [Required]
        public DateTime SalaryMonth { get; set; }

        [ForeignKey("Employee")]
        public int EmployeeId { get; set; }

        public Employee Employee { get; set; }
    }
}