using DepartmentModel = HRMS.Models.Department;

namespace HRMS.Repositories.Interfaces
{
    public interface IDepartmentRepository
    {
        Task<IEnumerable<DepartmentModel>> GetAllDepartmentsAsync();

        Task<DepartmentModel?> GetDepartmentByIdAsync(int id);

        Task AddDepartmentAsync(DepartmentModel department);

        Task UpdateDepartmentAsync(DepartmentModel department);

        Task DeleteDepartmentAsync(int id);
    }
}