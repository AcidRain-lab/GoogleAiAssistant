
using DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories.Implementations
{
    public class PhoneTypesRepository : IPhoneTypesRepository
    {
        private readonly CrmContext _context;
        public PhoneTypesRepository(CrmContext context)
        {
            _context = context;
        }

        public async Task<List<PhoneType>> GetPhoneTypesList()
        {
            var phoneTypeList = await _context.PhoneTypes.ToListAsync().ConfigureAwait(false);
            return phoneTypeList;
        }
    }
}
