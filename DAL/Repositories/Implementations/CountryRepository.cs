using DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories.Implementations
{
    public class CountryRepository : ICountryRepository
    {
        private readonly CrmContext _context;
        public CountryRepository(CrmContext context)
        {
            _context = context;
        }
        public async Task<List<Country>> GetCountriesList()
        {
            var countryList = await _context.Countries.ToListAsync().ConfigureAwait(false);
            return countryList;
        }
    }
}
