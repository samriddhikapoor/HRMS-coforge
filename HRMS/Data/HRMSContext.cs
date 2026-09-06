using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using HRMS.Models;

namespace HRMS.Data
{
    public class HRMSContext(DbContextOptions<HRMSContext> options)
        : IdentityDbContext<ApplicationUser>(options)
    {
        public DbSet<Department> Departments { get; set; }

        public DbSet<Employee> Employees { get; set; }

        public DbSet<Attendance> Attendances { get; set; }

        public DbSet<LeaveRequest> LeaveRequests { get; set; }

        public DbSet<Payroll> Payrolls { get; set; }

        public DbSet<PerformanceReview> PerformanceReviews { get; set; }

        public DbSet<Manager> Managers { get; set; }
    }
}