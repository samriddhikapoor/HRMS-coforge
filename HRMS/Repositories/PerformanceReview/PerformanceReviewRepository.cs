using HRMS.Data;
using HRMS.Models;
using HRMS.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HRMS.Repositories.Implementations
{
    public class PerformanceReviewRepository : IPerformanceReviewRepository
    {
        private readonly HRMSContext _context;

        public PerformanceReviewRepository(HRMSContext context)
        {
            _context = context;
        }

        // Get all performance reviews
        public async Task<IEnumerable<PerformanceReview>>
            GetAllPerformanceReviewsAsync()
        {
            return await _context.PerformanceReviews
                .Include(p => p.Employee)
                .OrderByDescending(p => p.ReviewDate)
                .ToListAsync();
        }

        // Get performance review by ID
        public async Task<PerformanceReview?>
            GetPerformanceReviewByIdAsync(int id)
        {
            return await _context.PerformanceReviews
                .Include(p => p.Employee)
                .FirstOrDefaultAsync(
                    p => p.PerformanceReviewId == id);
        }

        // Get reviews of a particular employee
        public async Task<IEnumerable<PerformanceReview>>
            GetPerformanceReviewsByEmployeeIdAsync(int employeeId)
        {
            return await _context.PerformanceReviews
                .Where(p => p.EmployeeId == employeeId)
                .OrderByDescending(p => p.ReviewDate)
                .ToListAsync();
        }

        // Add performance review
        public async Task AddPerformanceReviewAsync(
            PerformanceReview performanceReview)
        {
            await _context.PerformanceReviews
                .AddAsync(performanceReview);

            await _context.SaveChangesAsync();
        }

        // Update performance review
        public async Task UpdatePerformanceReviewAsync(
            PerformanceReview performanceReview)
        {
            _context.PerformanceReviews.Update(performanceReview);

            await _context.SaveChangesAsync();
        }

        // Delete performance review
        public async Task DeletePerformanceReviewAsync(int id)
        {
            var performanceReview =
                await _context.PerformanceReviews
                    .FirstOrDefaultAsync(
                        p => p.PerformanceReviewId == id);

            if (performanceReview != null)
            {
                _context.PerformanceReviews.Remove(performanceReview);

                await _context.SaveChangesAsync();
            }
        }
    }
}