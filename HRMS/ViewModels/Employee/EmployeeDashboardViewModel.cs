namespace HRMS.ViewModels.Employee
{
    public class EmployeeDashboardViewModel
    {
        public int EmployeeId { get; set; }

        public string EmployeeName { get; set; } = string.Empty;

        public string DepartmentName { get; set; } = string.Empty;

        public string Designation { get; set; } = string.Empty;

        public string TodayAttendanceStatus { get; set; } = "Not Marked";

        public int PendingLeaveRequests { get; set; }

        public decimal? LatestNetSalary { get; set; }

        public int? LatestPerformanceRating { get; set; }
    }
}