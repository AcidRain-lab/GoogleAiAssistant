using DAL.Models;

namespace DAL.Repositories
{
    public interface IContactRepository
    {
        Task<List<Contact>> GetContactsList();
        Task<Contact?> GetContactById(Guid id);
        Task<Contact?> GetContactByEmail(string email);
        Task<Guid> Save(Contact contact);
        Task<int> Update(Contact contact);
        Task<bool> EmailExists(string email);
        Task<bool> Delete(Guid id);
        Task<List<Contact>> GetContactsListByIds(List<Guid> contactGuids);
    }
}
