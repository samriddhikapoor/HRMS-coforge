using HRMS.Models;

namespace HRMS.Repositories.Interfaces
{
    public interface IPerformanceReviewRepository
    {
        Task<IEnumerable<PerformanceReview>> GetAllPerformanceReviewsAsync();

        Task<PerformanceReview?> GetPerformanceReviewByIdAsync(int id);

        Task<IEnumerable<PerformanceReview>>
            GetPerformanceReviewsByEmployeeIdAsync(int employeeId);

        Task AddPerformanceReviewAsync(PerformanceReview performanceReview);

        Task UpdatePerformanceReviewAsync(PerformanceReview performanceReview);

        Task DeletePerformanceReviewAsync(int id);
    }
}