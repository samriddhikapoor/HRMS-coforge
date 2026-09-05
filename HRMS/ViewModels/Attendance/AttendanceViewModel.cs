using System.ComponentModel.DataAnnotations;

namespace HRMS.ViewModels.Attendance
{
    public class AttendanceViewModel
    {
        public int AttendanceId { get; set; }

        [Required(ErrorMessage = "Attendance date is required")]
        public DateTime AttendanceDate { get; set; }

        [Required(ErrorMessage = "Status is required")]
        public string Status { get; set; } = string.Empty;

        public DateTime? CheckInTime { get; set; }

        public DateTime? CheckOutTime { get; set; }

        [Required(ErrorMessage = "Employee is required")]
        public int EmployeeId { get; set; }
    }
}