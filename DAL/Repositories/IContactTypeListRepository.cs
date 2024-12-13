using DAL.Models;

namespace DAL.Repositories
{
    public interface IContactTypeListRepository
    {
        Task<int> Save(ContactsTypesList contactTypesList);
        Task<List<int>> GetContactTypeIdsListByContactId(Guid contactId);
        Task<List<int>> GetContactTypeIdsList();
    }
}
