using BLL.DTO.Countries;
namespace BLL.Services
{
    public interface ICountryManager
    {
        Task<List<CountryDTO>> GetCountriesList();
    }
}
