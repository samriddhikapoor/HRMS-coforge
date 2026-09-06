using AutoMapper;
using HRMS.Models;
using HRMS.ViewModels.Payroll;

namespace HRMS.Mapping
{
    public class PayrollProfile : Profile
    {
        public PayrollProfile()
        {
            CreateMap<Payroll, PayrollViewModel>();
            CreateMap<PayrollViewModel, Payroll>();
        }
    }
}