using DAL.Models;

namespace DAL.Repositories
{
    public interface IPhoneRepository
    {
        Task<List<Phone>> GetPhoneListByAssociatedId(Guid associatedId);
        Task<bool> AddOrUpdatePhoneList(List<Phone> phones);
        Task<bool> DeletePhonesByIds(List<Guid> phoneIds);
    }
}
