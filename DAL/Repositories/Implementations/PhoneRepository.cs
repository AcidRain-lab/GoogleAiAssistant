
using DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories.Implementations
{
    public class PhoneRepository : IPhoneRepository
    {
        private readonly CrmContext _context;
        public PhoneRepository(CrmContext context)
        {
            _context = context;
        }

        public async Task<bool> AddOrUpdatePhoneList(List<Phone> phones)
        {
            foreach (var phone in phones)
            {
                if (phone.Id == Guid.Empty)
                {
                    await _context.Phones.AddRangeAsync(phone);
                }
                else
                {
                    _context.Phones.Update(phone);
                }
            }
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<Phone>> GetPhoneListByAssociatedId(Guid associatedId)
        {
            var phoneList = await _context.Phones.Where(phone => phone.AssociatedRecordId == associatedId).ToListAsync().ConfigureAwait(false);
            return phoneList;
        }

        public async Task<bool> DeletePhonesByIds(List<Guid> phoneIds)
        {
            var phonesToDelete = await _context.Phones.Where(p => phoneIds.Contains(p.Id)).ToListAsync().ConfigureAwait(false);
            _context.Phones.RemoveRange(phonesToDelete);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
