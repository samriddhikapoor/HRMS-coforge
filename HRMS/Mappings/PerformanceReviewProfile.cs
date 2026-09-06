using AutoMapper;
using HRMS.Models;
using HRMS.ViewModels.PerformanceReview;

namespace HRMS.Mappings
{
    public class PerformanceReviewProfile : Profile
    {
        public PerformanceReviewProfile()
        {
            CreateMap<PerformanceReviewViewModel, PerformanceReview>();

            CreateMap<PerformanceReview, PerformanceReviewViewModel>();
        }
    }
}