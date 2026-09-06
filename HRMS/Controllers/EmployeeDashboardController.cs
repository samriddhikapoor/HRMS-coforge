using HRMS.Models;
using HRMS.Repositories.Interfaces;
using HRMS.ViewModels.LeaveRequest;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HRMS.Controllers
{
    [Authorize(Roles = "Employee")]
    public class EmployeeDashboardController : Controller
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IAttendanceRepository _attendanceRepository;
        private readonly ILeaveRequestRepository _leaveRequestRepository;
        private readonly IPayrollRepository _payrollRepository;
        private readonly IPerformanceReviewRepository _performanceReviewRepository;

        public EmployeeDashboardController(
    IEmployeeRepository employeeRepository,
    IAttendanceRepository attendanceRepository,
    ILeaveRequestRepository leaveRequestRepository,
    IPayrollRepository payrollRepository,
    IPerformanceReviewRepository performanceReviewRepository)
        {
            _employeeRepository = employeeRepository;
            _attendanceRepository = attendanceRepository;
            _leaveRequestRepository = leaveRequestRepository;
            _payrollRepository = payrollRepository;
            _performanceReviewRepository = performanceReviewRepository;
        }

        // GET: EmployeeDashboard
        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
            {
                return Unauthorized();
            }

            var employee =
                await _employeeRepository.GetEmployeeByUserIdAsync(userId);

            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        // GET: EmployeeDashboard/Profile
        public async Task<IActionResult> Profile()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
            {
                return Unauthorized();
            }

            var employee =
                await _employeeRepository.GetEmployeeByUserIdAsync(userId);

            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        // GET: EmployeeDashboard/Attendance
        public async Task<IActionResult> Attendance()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
            {
                return Unauthorized();
            }

            var attendance =
                await _attendanceRepository
                    .GetAttendanceByUserIdAsync(userId);

            return View(attendance);
        }

        // POST: EmployeeDashboard/CheckIn
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CheckIn()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
            {
                return Unauthorized();
            }

            var employee =
                await _employeeRepository.GetEmployeeByUserIdAsync(userId);

            if (employee == null)
            {
                return NotFound();
            }

            var todayAttendance =
                await _attendanceRepository
                    .GetTodayAttendanceByUserIdAsync(userId);

            // Already checked in
            if (todayAttendance != null)
            {
                TempData["AttendanceMessage"] =
                    "You have already checked in today.";

                return RedirectToAction(nameof(Attendance));
            }

            var attendance = new Attendance
            {
                EmployeeId = employee.EmployeeId,
                AttendanceDate = DateTime.Today,
                Status = "Present",
                CheckInTime = DateTime.Now
            };

            await _attendanceRepository.AddAttendanceAsync(attendance);

            TempData["AttendanceMessage"] =
                "Check-in successful.";

            return RedirectToAction("Attendance");
        }

        // POST: EmployeeDashboard/CheckOut
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CheckOut()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
            {
                return Unauthorized();
            }

            var todayAttendance =
                await _attendanceRepository
                    .GetTodayAttendanceByUserIdAsync(userId);

            if (todayAttendance == null)
            {
                TempData["AttendanceMessage"] =
                    "Please check in first.";

                return RedirectToAction("Attendance");
            }

            if (todayAttendance.CheckOutTime != null)
            {
                TempData["AttendanceMessage"] =
                    "You have already checked out today.";

                return RedirectToAction("Attendance");
            }

            todayAttendance.CheckOutTime = DateTime.Now;

            await _attendanceRepository
                .UpdateAttendanceAsync(todayAttendance);

            TempData["AttendanceMessage"] =
                "Check-out successful.";

            return RedirectToAction("Attendance");
        }

        // GET: EmployeeDashboard/Leaves
        public async Task<IActionResult> Leaves()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
            {
                return Unauthorized();
            }

            var employee =
                await _employeeRepository.GetEmployeeByUserIdAsync(userId);

            if (employee == null)
            {
                return NotFound();
            }

            var leaves =
                await _leaveRequestRepository
                    .GetLeaveRequestsByEmployeeIdAsync(employee.EmployeeId);

            return View(leaves);
        }

        // GET: EmployeeDashboard/ApplyLeave
        [HttpGet]
        public IActionResult ApplyLeave()
        {
            return View();
        }

        // POST: EmployeeDashboard/ApplyLeave
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ApplyLeave(
            LeaveRequestViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
            {
                return Unauthorized();
            }

            var employee =
                await _employeeRepository.GetEmployeeByUserIdAsync(userId);

            if (employee == null)
            {
                return NotFound();
            }

            var leaveRequest = new LeaveRequest
            {
                LeaveType = model.LeaveType,
                StartDate = model.StartDate,
                EndDate = model.EndDate,
                Reason = model.Reason,
                Status = "Pending",
                EmployeeId = employee.EmployeeId
            };

            await _leaveRequestRepository
                .AddLeaveRequestAsync(leaveRequest);

            return RedirectToAction("Leaves");
        }

        // GET: EmployeeDashboard/Payroll
        public async Task<IActionResult> Payroll()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
            {
                return Unauthorized();
            }

            var employee =
                await _employeeRepository.GetEmployeeByUserIdAsync(userId);

            if (employee == null)
            {
                return NotFound();
            }

            var payrolls =
                await _payrollRepository
                    .GetPayrollsByEmployeeIdAsync(employee.EmployeeId);

            return View(payrolls);
        }
        // GET: EmployeeDashboard/Performance
        public async Task<IActionResult> Performance()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
            {
                return Unauthorized();
            }

            var employee =
                await _employeeRepository.GetEmployeeByUserIdAsync(userId);

            if (employee == null)
            {
                return NotFound();
            }

            var reviews =
                await _performanceReviewRepository
                    .GetPerformanceReviewsByEmployeeIdAsync(employee.EmployeeId);

            return View(reviews);
        }
    }
}