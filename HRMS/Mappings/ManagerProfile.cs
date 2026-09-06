

using AutoMapper;
using HRMS.Models;
using HRMS.ViewModels.Manager;

namespace HRMS.Mapping
{
    public class ManagerProfile : Profile
    {
        public ManagerProfile()
        {
            // Entity → ViewModel
            CreateMap<Manager, ManagerViewModel>();

            // ViewModel → Entity
            CreateMap<ManagerViewModel, Manager>();
        }
    }
}