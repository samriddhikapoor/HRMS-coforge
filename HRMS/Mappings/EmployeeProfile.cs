using AutoMapper;
using HRMS.Models;
using HRMS.ViewModels.Employee;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace HRMS.Mappings
{
    public class EmployeeProfile : Profile
    {
        public EmployeeProfile()
        {
            CreateMap<Employee, EmployeeViewModel>();
            CreateMap<EmployeeViewModel, Employee>();
        }
    }
}