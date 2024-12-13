using DAL.Models;

namespace DAL.Repositories
{
    public interface IContactTypeRepository
    {
        Task<List<ContactsType>> GetContactTypeList();
    }
}
