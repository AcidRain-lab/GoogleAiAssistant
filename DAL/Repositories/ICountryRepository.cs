using DAL.Models;

namespace DAL.Repositories
{
    public interface ICountryRepository
    {
        Task<List<Country>> GetCountriesList();
    }
}
