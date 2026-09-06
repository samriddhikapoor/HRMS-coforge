using HRMS.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HRMS.Controllers
{
    [Authorize(Roles = "HR,Admin,Manager")]
    public class LeaveRequestController : Controller
    {
        private readonly ILeaveRequestRepository _leaveRequestRepository;

        public LeaveRequestController(
            ILeaveRequestRepository leaveRequestRepository)
        {
            _leaveRequestRepository = leaveRequestRepository;
        }

        // GET: LeaveRequest
        public async Task<IActionResult> Index()
        {
            var leaveRequests =
                await _leaveRequestRepository.GetAllLeaveRequestsAsync();

            return View(leaveRequests);
        }

        // GET: LeaveRequest/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var leaveRequest =
                await _leaveRequestRepository.GetLeaveRequestByIdAsync(id);

            if (leaveRequest == null)
            {
                return NotFound();
            }

            return View(leaveRequest);
        }

        // POST: LeaveRequest/Approve/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Approve(int id)
        {
            var leaveRequest =
                await _leaveRequestRepository.GetLeaveRequestByIdAsync(id);

            if (leaveRequest == null)
            {
                return NotFound();
            }

            leaveRequest.Status = "Approved";

            await _leaveRequestRepository
                .UpdateLeaveRequestAsync(leaveRequest);

            return RedirectToAction(nameof(Index));
        }

        // POST: LeaveRequest/Reject/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Reject(int id)
        {
            var leaveRequest =
                await _leaveRequestRepository.GetLeaveRequestByIdAsync(id);

            if (leaveRequest == null)
            {
                return NotFound();
            }

            leaveRequest.Status = "Rejected";

            await _leaveRequestRepository
                .UpdateLeaveRequestAsync(leaveRequest);

            return RedirectToAction(nameof(Index));
        }
    }
}