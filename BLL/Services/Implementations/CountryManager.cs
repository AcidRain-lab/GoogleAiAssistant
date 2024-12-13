using BLL.DTO.Countries;
using DAL.Repositories;

namespace BLL.Services.Implementations
{
    public class CountryManager : ICountryManager
    {
        private readonly ICountryRepository _countryRepository;
        public CountryManager(ICountryRepository countryRepository)
        {
                _countryRepository = countryRepository;
        }

        public async Task<List<CountryDTO>> GetCountriesList()
        {
            var countryList = await _countryRepository.GetCountriesList();
            List<CountryDTO> countries = new();
            if (countryList.Any())
            {
                countryList.ForEach(country => 
                {
                    CountryDTO countryDTO = new()
                    {
                        CountryId = country.Id,
                        Name = country.Name,
                    };
                    countries.Add(countryDTO);
                });
            }
            return countries;
        }
    }
}
