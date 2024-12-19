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
            CreateMap<Client, ClientDTO>()
               .ReverseMap();


            CreateMap<Individual, IndividualDTO>().ReverseMap();
            CreateMap<Organization, OrganizationDTO>().ReverseMap();
            CreateMap<TermsAndRule, TermsAndRulesDto>()
                .ForMember(dest => dest.SubTermsAndRules, opt => opt.MapFrom(src => src.SubTermsAndRules));

            CreateMap<SubTermsAndRule, SubTermsAndRulesDto>()
                .ForMember(dest => dest.NestedSubTerms, opt => opt.MapFrom(src => src.NestedSubTerms));

            CreateMap<NestedSubTerm, NestedSubTermDto>();
        }
    }
}
