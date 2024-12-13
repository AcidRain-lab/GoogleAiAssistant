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
        }
    }
}
