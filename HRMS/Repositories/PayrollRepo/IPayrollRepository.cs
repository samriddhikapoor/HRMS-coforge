using PayrollModel=HRMS.Models.Payroll;


namespace HRMS.Repositories.Interfaces
{
    public interface IPayrollRepository
    {
        Task<IEnumerable<PayrollModel>> GetAllPayrollsAsync();

        Task<PayrollModel?> GetPayrollByIdAsync(int id);

        Task<IEnumerable<PayrollModel>> GetPayrollsByEmployeeIdAsync(int employeeId);

        Task AddPayrollAsync(PayrollModel payroll);

        Task UpdatePayrollAsync(PayrollModel payroll);

        Task DeletePayrollAsync(int id);
    }
}