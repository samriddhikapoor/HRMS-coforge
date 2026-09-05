using HRMS.Data;
using HRMS.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using DepartmentModel = HRMS.Models.Department;

namespace HRMS.Repositories.Implementations
{
    public class DepartmentRepository : IDepartmentRepository
    {
        private readonly HRMSContext _context;

        public DepartmentRepository(HRMSContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<DepartmentModel>> GetAllDepartmentsAsync()
        {
            return await _context.Departments
                .OrderBy(d => d.DepartmentName)
                .ToListAsync();
        }

        public async Task<DepartmentModel?> GetDepartmentByIdAsync(int id)
        {
            return await _context.Departments
                .FirstOrDefaultAsync(d => d.DepartmentId == id);
        }

        public async Task AddDepartmentAsync(DepartmentModel department)
        {
            await _context.Departments.AddAsync(department);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateDepartmentAsync(DepartmentModel department)
        {
            _context.Departments.Update(department);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteDepartmentAsync(int id)
        {
            var department = await GetDepartmentByIdAsync(id);

            if (department != null)
            {
                _context.Departments.Remove(department);
                await _context.SaveChangesAsync();
            }
        }
    }
}