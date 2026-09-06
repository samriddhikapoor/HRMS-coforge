using HRMS.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HRMS.Controllers
{
    [Authorize(Roles = "HR")]
    public class HRDashboardController : Controller
    {
        private readonly IAttendanceRepository _attendanceRepository;
        private readonly ILeaveRequestRepository _leaveRequestRepository;
        private readonly IPayrollRepository _payrollRepository;
        private readonly IPerformanceReviewRepository _performanceReviewRepository;

        public HRDashboardController(
            IAttendanceRepository attendanceRepository,
            ILeaveRequestRepository leaveRequestRepository,
            IPayrollRepository payrollRepository,
            IPerformanceReviewRepository performanceReviewRepository)
        {
            _attendanceRepository = attendanceRepository;
            _leaveRequestRepository = leaveRequestRepository;
            _payrollRepository = payrollRepository;
            _performanceReviewRepository = performanceReviewRepository;
        }

        // HR Dashboard
        public IActionResult Index()
        {
            return View();
        }

        // HR - All Attendance
        public async Task<IActionResult> Attendance()
        {
            var attendance =
                await _attendanceRepository
                    .GetAllAttendancesAsync();

            return View(attendance);
        }

        // HR - All Leave Requests
        public async Task<IActionResult> Leaves()
        {
            var leaves =
                await _leaveRequestRepository
                    .GetAllLeaveRequestsAsync();

            return View(leaves);
        }

        // HR - All Payroll
        public async Task<IActionResult> Payroll()
        {
            var payrolls =
                await _payrollRepository
                    .GetAllPayrollsAsync();

            return View(payrolls);
        }

        // HR - All Performance Reviews
        public async Task<IActionResult> Performance()
        {
            var reviews =
                await _performanceReviewRepository
                    .GetAllPerformanceReviewsAsync();

            return View(reviews);
        }
    }
}