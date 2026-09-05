using AutoMapper;
using HRMS.Data;
using HRMS.ViewModels.User;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace HRMS.Mappings
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<ApplicationUser, CreateUserViewModel>();
            CreateMap<CreateUserViewModel, ApplicationUser>();
        }
    }
}