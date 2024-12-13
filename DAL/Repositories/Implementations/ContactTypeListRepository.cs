using DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories.Implementations
{
    public class ContactTypeListRepository : IContactTypeListRepository
    {
        private readonly CrmContext _context;
        public ContactTypeListRepository(CrmContext context)
        {
            _context = context;
        }

        public async Task<List<int>> GetContactTypeIdsList()
        {
            List<int> contactIds = await _context.ContactsTypesLists.Select(x => x.ContactTypeId).ToListAsync();
            return contactIds;
        }

        public async Task<List<int>> GetContactTypeIdsListByContactId(Guid contactId)
        {
            List<int> contactIds = new List<int>();
            var contactIdsList = await _context.ContactsTypesLists.Where(x => x.ContactId == contactId).Select(x => x.ContactTypeId).ToListAsync();
            contactIds.AddRange(contactIdsList);
            return contactIds;
        }

        public async Task<int> Save(ContactsTypesList contactTypesList)
        {
            await _context.ContactsTypesLists.AddAsync(contactTypesList).ConfigureAwait(false);
            return await _context.SaveChangesAsync();

        }
    }
}
