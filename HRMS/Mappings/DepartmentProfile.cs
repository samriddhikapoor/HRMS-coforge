using AutoMapper;
using HRMS.Models;
using HRMS.ViewModels.Department;

namespace HRMS.Mappings
{
    public class DepartmentProfile : Profile
    {
        public DepartmentProfile()
        {
            CreateMap<Department, DepartmentViewModel>();
            CreateMap<DepartmentViewModel, Department>();
        }
    }
}