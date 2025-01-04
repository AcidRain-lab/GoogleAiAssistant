using AutoMapper;
using DAL.Models;
using WebObjectsBLL.DTO;

namespace WebObjectsBLL
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Mapping for Clients
            CreateMap<Client, ClientDTO>().ReverseMap();



            CreateMap<Client, ClientDetailDTO>()
                .ForMember(dest => dest.BirthDate, opt => opt.MapFrom(src => src.BirthDate.HasValue ? src.BirthDate.Value.ToDateTime(TimeOnly.MinValue) : (DateTime?)null))
                .ReverseMap()
                .ForMember(dest => dest.BirthDate, opt => opt.MapFrom(src => src.BirthDate.HasValue ? DateOnly.FromDateTime(src.BirthDate.Value) : (DateOnly?)null));

            // Mapping for Transactions
            CreateMap<Transaction, TransactionDTO>().ReverseMap();

            // Mapping for Deposits
            CreateMap<Deposit, DepositDTO>()
                 .ForMember(dest => dest.MaturityDate, opt => opt.MapFrom(src => src.MaturityDate.ToDateTime(TimeOnly.MinValue)))
                 .ReverseMap()
                 .ForMember(dest => dest.MaturityDate, opt => opt.MapFrom(src => DateOnly.FromDateTime(src.MaturityDate)));

            // Mapping for Regular Payments
            CreateMap<RegularPayment, RegularPaymentDTO>()
                .ForMember(dest => dest.NextPaymentDate, opt => opt.MapFrom(src => src.NextPaymentDate.ToDateTime(TimeOnly.MinValue)))
                .ReverseMap()
                .ForMember(dest => dest.NextPaymentDate, opt => opt.MapFrom(src => DateOnly.FromDateTime(src.NextPaymentDate)));

            // Mapping for Cashbacks
            CreateMap<Cashback, CashbackDTO>().ReverseMap();

            // Mapping for Messages
            CreateMap<Message, MessageDTO>().ReverseMap();

            // Mapping for Credits
            /* CreateMap<Credit, CreditDTO>()
                 .ForMember(dest => dest.StartDate, opt => opt.MapFrom(src => src.StartDate.ToDateTime(TimeOnly.MinValue)))
                 .ForMember(dest => dest.EndDate, opt => opt.MapFrom(src => src.EndDate.ToDateTime(TimeOnly.MinValue)))
                 .ReverseMap()
                 .ForMember(dest => dest.StartDate, opt => opt.MapFrom(src => DateOnly.FromDateTime(src.StartDate)))
                 .ForMember(dest => dest.EndDate, opt => opt.MapFrom(src => DateOnly.FromDateTime(src.EndDate)))
                 .ForMember(dest => dest.CreditType, opt => opt.Ignore()); // Exclude CreditType relationship

             // Mapping for CreditType
             CreateMap<CreditType, CreditTypeDTO>().ReverseMap();*/
            CreateMap<CreditType, CreditTypeDTO>()
    .ForMember(dest => dest.Documents, opt => opt.Ignore()) // Если документы не нужны
    .ReverseMap();

            CreateMap<Credit, CreditDTO>()
                .ForMember(dest => dest.StartDate, opt => opt.MapFrom(src => src.StartDate.ToDateTime(TimeOnly.MinValue)))
                .ForMember(dest => dest.EndDate, opt => opt.MapFrom(src => src.EndDate.ToDateTime(TimeOnly.MinValue)))
                .ReverseMap()
                .ForMember(dest => dest.StartDate, opt => opt.MapFrom(src => DateOnly.FromDateTime(src.StartDate)))
                .ForMember(dest => dest.EndDate, opt => opt.MapFrom(src => DateOnly.FromDateTime(src.EndDate)));


            CreateMap<DepositTypeDetailDTO, DepositType>()
                .ForMember(dest => dest.DepositTerms, opt => opt.Ignore()) // Пропустить внутренние коллекции, если они не изменяются
                .ReverseMap()
                .ForMember(dest => dest.Avatar, opt => opt.Ignore())
                .ForMember(dest => dest.MediaFiles, opt => opt.Ignore())
                .ForMember(dest => dest.Documents, opt => opt.Ignore());

            CreateMap<DepositTerm, DepositTermDTO>().ReverseMap();

            // Mapping for NestedSubTerm
            CreateMap<NestedSubTerm, NestedSubTermDto>().ReverseMap();

            // Mapping for SubTermsAndRules
            CreateMap<SubTermsAndRule, SubTermsAndRulesDto>()
                .ForMember(dest => dest.NestedSubTerms, opt => opt.MapFrom(src => src.NestedSubTerms))
                .ReverseMap()
                .ForMember(dest => dest.NestedSubTerms, opt => opt.MapFrom(src => src.NestedSubTerms));

            // Mapping for TermsAndRules
            CreateMap<TermsAndRule, TermsAndRulesDto>()
                .ForMember(dest => dest.SubTermsAndRules, opt => opt.MapFrom(src => src.SubTermsAndRules))
                .ReverseMap()
                .ForMember(dest => dest.SubTermsAndRules, opt => opt.MapFrom(src => src.SubTermsAndRules));

            // Mapping for BankCard
            CreateMap<BankCard, BankCardDTO>()
                .ForMember(dest => dest.ExpirationDate, opt => opt.MapFrom(src => src.ExpirationDate.ToDateTime(TimeOnly.MinValue)))
                .ForMember(dest => dest.CardTypeName, opt => opt.MapFrom(src => src.CardType.Name))
                .ReverseMap()
                .ForMember(dest => dest.ExpirationDate, opt => opt.MapFrom(src => DateOnly.FromDateTime(src.ExpirationDate)))
                .ForMember(dest => dest.BankAccount, opt => opt.Ignore()) // Ensure no automatic BankAccount creation
                .ForMember(dest => dest.CardType, opt => opt.Ignore());

            // Mapping for BankAccount
            CreateMap<BankAccount, BankAccountDTO>()
                .ForMember(dest => dest.OpenedDate, opt => opt.MapFrom(src => src.OpenedDate.ToDateTime(TimeOnly.MinValue)))
                .ForMember(dest => dest.ClosedDate, opt => opt.MapFrom(src => src.ClosedDate.HasValue ? src.ClosedDate.Value.ToDateTime(TimeOnly.MinValue) : (DateTime?)null))
                .ReverseMap()
                .ForMember(dest => dest.OpenedDate, opt => opt.MapFrom(src => DateOnly.FromDateTime(src.OpenedDate)))
                .ForMember(dest => dest.ClosedDate, opt => opt.MapFrom(src => src.ClosedDate.HasValue ? DateOnly.FromDateTime(src.ClosedDate.Value) : (DateOnly?)null))
                .ForMember(dest => dest.Client, opt => opt.Ignore()); // Ensure no automatic Client creation

            // Mapping for CardType -> CardTypeTableDTO
            CreateMap<CardType, CardTypeTableDTO>()
               .ForMember(dest => dest.PaymentSystemTypeName, opt => opt.MapFrom(src => src.PaymentSystemType != null ? src.PaymentSystemType.Name : string.Empty));

            // Mapping for CardType -> CardTypeDetailDTO
            CreateMap<CardType, CardTypeDetailDTO>()
                .ForMember(dest => dest.PaymentSystemTypeName, opt => opt.MapFrom(src => src.PaymentSystemType != null ? src.PaymentSystemType.Name : string.Empty))
                .ForMember(dest => dest.Avatar, opt => opt.Ignore()) // Exclude Avatar logic
                .ForMember(dest => dest.MediaFiles, opt => opt.Ignore()) // Exclude MediaFiles logic
                .ForMember(dest => dest.Documents, opt => opt.Ignore()) // Exclude Documents logic
                .ReverseMap()
                .ForMember(dest => dest.PaymentSystemType, opt => opt.Ignore()); // Ignore PaymentSystemType on update

            // Mapping for PaymentSystemType
            CreateMap<PaymentSystemType, PaymentSystemTypeDTO>().ReverseMap();
        }
    }
}
