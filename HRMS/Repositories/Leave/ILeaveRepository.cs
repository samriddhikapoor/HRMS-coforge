using LeaveRequest = HRMS.Models.LeaveRequest;


namespace HRMS.Repositories.Interfaces
{
    public interface ILeaveRequestRepository
    {
        Task<IEnumerable<LeaveRequest>> GetAllLeaveRequestsAsync();

        Task<LeaveRequest?> GetLeaveRequestByIdAsync(int id);

        Task<IEnumerable<LeaveRequest>> GetLeaveRequestsByEmployeeIdAsync(int employeeId);

        Task AddLeaveRequestAsync(LeaveRequest leaveRequest);

        Task UpdateLeaveRequestAsync(LeaveRequest leaveRequest);

        Task DeleteLeaveRequestAsync(int id);
    }
}