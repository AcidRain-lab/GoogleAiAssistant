using AutoMapper;
using DAL.Models;
using WebObjectsBLL.DTO;

namespace WebObjectsBLL
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Маппинг для Clients
            CreateMap<Client, ClientDTO>().ReverseMap();

            CreateMap<Client, ClientDetailDTO>()
                .ForMember(dest => dest.BirthDate, opt => opt.MapFrom(src => src.BirthDate.HasValue ? src.BirthDate.Value.ToDateTime(TimeOnly.MinValue) : (DateTime?)null))
                .ReverseMap()
                .ForMember(dest => dest.BirthDate, opt => opt.MapFrom(src => src.BirthDate.HasValue ? DateOnly.FromDateTime(src.BirthDate.Value) : (DateOnly?)null));

            // Маппинг для Transactions
            CreateMap<Transaction, TransactionDTO>().ReverseMap();

            // Маппинг для Deposits
            CreateMap<Deposit, DepositDTO>()
                 .ForMember(dest => dest.MaturityDate, opt => opt.MapFrom(src => src.MaturityDate.ToDateTime(TimeOnly.MinValue)))
                 .ReverseMap()
                 .ForMember(dest => dest.MaturityDate, opt => opt.MapFrom(src => DateOnly.FromDateTime(src.MaturityDate)));

            // Маппинг для Regular Payments
            CreateMap<RegularPayment, RegularPaymentDTO>()
                .ForMember(dest => dest.NextPaymentDate, opt => opt.MapFrom(src => src.NextPaymentDate.ToDateTime(TimeOnly.MinValue)))
                .ReverseMap()
                .ForMember(dest => dest.NextPaymentDate, opt => opt.MapFrom(src => DateOnly.FromDateTime(src.NextPaymentDate)));

            // Маппинг для Cashbacks
            CreateMap<Cashback, CashbackDTO>().ReverseMap();

            // Маппинг для Messages
            CreateMap<Message, MessageDTO>().ReverseMap();

            // Маппинг для Credits
            CreateMap<Credit, CreditDTO>()
                .ForMember(dest => dest.StartDate, opt => opt.MapFrom(src => src.StartDate.ToDateTime(TimeOnly.MinValue)))
                .ForMember(dest => dest.EndDate, opt => opt.MapFrom(src => src.EndDate.ToDateTime(TimeOnly.MinValue)))
                .ReverseMap()
                .ForMember(dest => dest.StartDate, opt => opt.MapFrom(src => DateOnly.FromDateTime(src.StartDate)))
                .ForMember(dest => dest.EndDate, opt => opt.MapFrom(src => DateOnly.FromDateTime(src.EndDate)));

            // Маппинг для NestedSubTerm
            CreateMap<NestedSubTerm, NestedSubTermDto>().ReverseMap();

            // Маппинг для SubTermsAndRules
            CreateMap<SubTermsAndRule, SubTermsAndRulesDto>()
                .ForMember(dest => dest.NestedSubTerms, opt => opt.MapFrom(src => src.NestedSubTerms))
                .ReverseMap()
                .ForMember(dest => dest.NestedSubTerms, opt => opt.MapFrom(src => src.NestedSubTerms));

            // Маппинг для TermsAndRules
            CreateMap<TermsAndRule, TermsAndRulesDto>()
                .ForMember(dest => dest.SubTermsAndRules, opt => opt.MapFrom(src => src.SubTermsAndRules))
                .ReverseMap()
                .ForMember(dest => dest.SubTermsAndRules, opt => opt.MapFrom(src => src.SubTermsAndRules));
        }
    }
}
