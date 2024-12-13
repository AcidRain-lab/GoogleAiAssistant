
using BLL.DTO;

namespace BLL.Services
{
    public interface IPhoneManager
    {
        Task<List<PhoneDTO>> GetPhoneListByAssociatedId(Guid associatedId);
        Task<bool> DeletePhonesByIds(List<Guid> phoneIds);
    }
}
