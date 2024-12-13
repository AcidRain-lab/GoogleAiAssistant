using BLL.DTO.Addresses;
using DAL.Repositories;
namespace BLL.Services.Implementations
{
    public class LocationManager : ILocationManager
    {
        private readonly ILocationRepository _locationRepository;
        public LocationManager(ILocationRepository locationRepository)
        {
            _locationRepository = locationRepository;
        }

        public async Task<AddressSummaryDTO> GetLocationById(Guid locationId)
        {
            var locationSummary = await _locationRepository.GetLocationById(locationId);
            AddressSummaryDTO addressSummaryDTO = new();
            if (locationSummary != null)
            {
                addressSummaryDTO.Id = locationSummary.Id;
                addressSummaryDTO.City = locationSummary.City;
                addressSummaryDTO.CountryId = locationSummary.CountryId;
                addressSummaryDTO.StateId = locationSummary.StateId;
                addressSummaryDTO.Street = locationSummary.Street;
                addressSummaryDTO.SuiteAptUnit = locationSummary.SuiteAptUnit;
                addressSummaryDTO.Zip = locationSummary.Zip.GetValueOrDefault();
            };
            return addressSummaryDTO;
        }

    }
}

