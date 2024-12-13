using DAL.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BLL
{
    public class ContactsTypeManager
    {
        private readonly CrmContext _context;

        public ContactsTypeManager(CrmContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ContactsType>> GetAllAsync()
        {
            return await _context.ContactsTypes.ToListAsync();
        }

        public async Task<ContactsType> GetByIdAsync(int id)
        {
            return await _context.ContactsTypes.FirstOrDefaultAsync(ct => ct.Id == id);
        }

        public async Task AddAsync(ContactsType contactsType)
        {
            _context.ContactsTypes.Add(contactsType);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(ContactsType contactsType)
        {
            _context.ContactsTypes.Update(contactsType);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var contactsType = await _context.ContactsTypes.FindAsync(id);
            if (contactsType != null)
            {
                _context.ContactsTypes.Remove(contactsType);
                await _context.SaveChangesAsync();
            }
        }
    }
}
