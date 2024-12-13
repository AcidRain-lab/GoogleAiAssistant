using BLL.DTO;
using BLL.DTO.Phones;

namespace BLL.Services
{
    public interface IPhoneTypeManager
    {
        Task<List<PhoneTypeSummaryDTO>> GetPhoneTypeList();
    }
}
