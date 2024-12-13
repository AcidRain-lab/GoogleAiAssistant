using DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories.Implementations
{
    public class ContactTypeRepository : IContactTypeRepository
    {
        private readonly CrmContext _context;
        public ContactTypeRepository(CrmContext context)
        {
                _context = context;
        }
        public async Task<List<ContactsType>> GetContactTypeList()
        {
            var contactTypeList = await _context.ContactsTypes.ToListAsync().ConfigureAwait(false);
            return contactTypeList;
        }
    }
}
