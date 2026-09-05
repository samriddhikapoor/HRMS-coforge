using HRMS.Models;
using EmployeeModel = HRMS.Models.Employee;

namespace HRMS.Repositories.Interfaces
{
    public interface IEmployeeRepository
    {
        Task<IEnumerable<EmployeeModel>> GetEmployeesAsync();

        Task<EmployeeModel?> GetEmployeeByIdAsync(int employeeId);

        Task AddEmployeeAsync(EmployeeModel employee);

        Task UpdateEmployeeAsync(EmployeeModel employee);

        Task DeleteEmployeeAsync(int employeeId);
        Task<Employee?> GetEmployeeByUserIdAsync(string userId);
    }
}