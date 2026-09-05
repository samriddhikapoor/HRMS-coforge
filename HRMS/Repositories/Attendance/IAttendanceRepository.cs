using HRMS.Models;

namespace HRMS.Repositories.Interfaces
{
    public interface IAttendanceRepository
    {
        Task<IEnumerable<Attendance>> GetAllAttendancesAsync();

        Task<Attendance?> GetAttendanceByIdAsync(int id);

        Task<IEnumerable<Attendance>> GetAttendanceByUserIdAsync(string userId);

        Task<Attendance?> GetTodayAttendanceByUserIdAsync(string userId);

        Task AddAttendanceAsync(Attendance attendance);

        Task UpdateAttendanceAsync(Attendance attendance);

        Task DeleteAttendanceAsync(int id);
    }
}