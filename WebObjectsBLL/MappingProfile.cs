using AutoMapper;
using DAL.Models;
using WebObjectsBLL.DTO;

namespace WebObjectsBLL
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Transaction, TransactionDTO>().ReverseMap();
            CreateMap<MediaDatum, MediaDataDTO>().ReverseMap();
            CreateMap<Avatar, AvatarDTO>().ReverseMap();
            CreateMap<Phone, PhoneDTO>().ReverseMap();
            CreateMap<PhoneType, PhoneTypeDTO>().ReverseMap();
            CreateMap<Country, CountryDTO>().ReverseMap();
            CreateMap<DocumentsDatum, DocumentsDataDTO>().ReverseMap();
        }
    }
}
