using HRMS.Data;
using HRMS.Models;
using HRMS.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HRMS.Repositories.Implementations
{
    public class PayrollRepository : IPayrollRepository
    {
        private readonly HRMSContext _context;

        public PayrollRepository(HRMSContext context)
        {
            _context = context;
        }

        // Get all payroll records
        public async Task<IEnumerable<Payroll>> GetAllPayrollsAsync()
        {
            return await _context.Payrolls
                .Include(p => p.Employee)
                .OrderByDescending(p => p.SalaryMonth)
                .ToListAsync();
        }

        // Get payroll by ID
        public async Task<Payroll?> GetPayrollByIdAsync(int id)
        {
            return await _context.Payrolls
                .Include(p => p.Employee)
                .FirstOrDefaultAsync(p => p.PayrollId == id);
        }

        // Get payroll records of a particular employee
        public async Task<IEnumerable<Payroll>> GetPayrollsByEmployeeIdAsync(
            int employeeId)
        {
            return await _context.Payrolls
                .Where(p => p.EmployeeId == employeeId)
                .OrderByDescending(p => p.SalaryMonth)
                .ToListAsync();
        }

        // Add payroll
        public async Task AddPayrollAsync(Payroll payroll)
        {
            await _context.Payrolls.AddAsync(payroll);
            await _context.SaveChangesAsync();
        }

        // Update payroll
        public async Task UpdatePayrollAsync(Payroll payroll)
        {
            _context.Payrolls.Update(payroll);
            await _context.SaveChangesAsync();
        }

        // Delete payroll
        public async Task DeletePayrollAsync(int id)
        {
            var payroll = await _context.Payrolls
                .FirstOrDefaultAsync(p => p.PayrollId == id);

            if (payroll != null)
            {
                _context.Payrolls.Remove(payroll);
                await _context.SaveChangesAsync();
            }
        }
    }
}