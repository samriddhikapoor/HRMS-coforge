using HRMS.Data;
using HRMS.Models;
using HRMS.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HRMS.Repositories.Implementations
{
    public class AttendanceRepository : IAttendanceRepository
    {
        private readonly HRMSContext _context;

        public AttendanceRepository(HRMSContext context)
        {
            _context = context;
        }

        // Get all attendance records
        public async Task<IEnumerable<Attendance>> GetAllAttendancesAsync()
        {
            return await _context.Attendances
                .Include(a => a.Employee)
                .OrderByDescending(a => a.AttendanceDate)
                .ToListAsync();
        }

        // Get attendance by ID
        public async Task<Attendance?> GetAttendanceByIdAsync(int id)
        {
            return await _context.Attendances
                .Include(a => a.Employee)
                .FirstOrDefaultAsync(a => a.AttendanceId == id);
        }

        // Get attendance of logged-in employee
        public async Task<IEnumerable<Attendance>> GetAttendanceByUserIdAsync(
            string userId)
        {
            return await _context.Attendances
                .Include(a => a.Employee)
                .Where(a => a.Employee.UserId == userId)
                .OrderByDescending(a => a.AttendanceDate)
                .ToListAsync();
        }

        // Get today's attendance of logged-in employee
        public async Task<Attendance?> GetTodayAttendanceByUserIdAsync(
            string userId)
        {
            var today = DateTime.Today;

            return await _context.Attendances
                .Include(a => a.Employee)
                .FirstOrDefaultAsync(a =>
                    a.Employee.UserId == userId &&
                    a.AttendanceDate.Date == today);
        }

        // Add attendance
        public async Task AddAttendanceAsync(Attendance attendance)
        {
            await _context.Attendances.AddAsync(attendance);
            await _context.SaveChangesAsync();
        }

        // Update attendance
        public async Task UpdateAttendanceAsync(Attendance attendance)
        {
            _context.Attendances.Update(attendance);
            await _context.SaveChangesAsync();
        }

        // Delete attendance
        public async Task DeleteAttendanceAsync(int id)
        {
            var attendance = await _context.Attendances
                .FirstOrDefaultAsync(a => a.AttendanceId == id);

            if (attendance != null)
            {
                _context.Attendances.Remove(attendance);
                await _context.SaveChangesAsync();
            }
        }
    }
}