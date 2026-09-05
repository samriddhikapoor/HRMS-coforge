using System.ComponentModel.DataAnnotations;

namespace HRMS.ViewModels.LeaveRequest
{
    public class LeaveRequestViewModel
    {
        public int LeaveRequestId { get; set; }

        [Required(ErrorMessage = "Leave type is required")]
        public string LeaveType { get; set; } = string.Empty;

        [Required(ErrorMessage = "Start date is required")]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "End date is required")]
        public DateTime EndDate { get; set; }

        [Required(ErrorMessage = "Reason is required")]
        [StringLength(500)]
        public string Reason { get; set; } = string.Empty;

        public string Status { get; set; } = "Pending";

        public int EmployeeId { get; set; }
    }
}