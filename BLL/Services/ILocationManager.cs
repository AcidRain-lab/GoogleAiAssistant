using BLL.DTO.Addresses;
namespace BLL.Services
{
    public interface ILocationManager
    {
        Task<AddressSummaryDTO> GetLocationById(Guid locationId);
    }
}
