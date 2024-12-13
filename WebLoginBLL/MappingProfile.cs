using AutoMapper;
using DAL.Models;
using WebLoginBLL.DTO;

namespace WebLoginBLL
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Маппинг User -> UserDTO
            CreateMap<User, UserDTO>()
                .ForMember(dest => dest.RoleName, opt => opt.MapFrom(src => src.Role.Name))
                .ReverseMap()
                .ForMember(dest => dest.Role, opt => opt.Ignore()) // Игнорируем Role при обратном маппинге
                .ForMember(dest => dest.RoleId, opt => opt.MapFrom(src => src.RoleId)); // Используем RoleId

            // Маппинг Role -> RoleDTO
            CreateMap<Role, RoleDTO>().ReverseMap();
        }
    }
}

