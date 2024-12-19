using AutoMapper;
using DAL.Models;
using WebObjectsBLL.DTO;

namespace WebObjectsBLL
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Client, ClientDTO>().ReverseMap();

            CreateMap<Client, ClientDetailDTO>()
                .ForMember(dest => dest.BirthDate, opt => opt.MapFrom(src => src.BirthDate.HasValue ? src.BirthDate.Value.ToDateTime(TimeOnly.MinValue) : (DateTime?)null))
                .ReverseMap()
                .ForMember(dest => dest.BirthDate, opt => opt.MapFrom(src => src.BirthDate.HasValue ? DateOnly.FromDateTime(src.BirthDate.Value) : (DateOnly?)null));

            CreateMap<Transaction, TransactionDTO>().ReverseMap();
        }
    }
}
