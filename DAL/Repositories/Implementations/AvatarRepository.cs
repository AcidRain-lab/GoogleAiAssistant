using DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories.Implementations
{
    public class AvatarRepository : IAvatarRepository
    {
        private readonly CrmContext _context;
        public AvatarRepository(CrmContext context)
        {
            _context = context;
        }

        public async Task<List<Avatar>> GetAll()
        {
            return await _context.Avatars.ToListAsync().ConfigureAwait(false);
        }

        public async Task<Avatar> GetAvatarByAssociatedRecordId(Guid associtedID)
        {
            var avatar = await _context.Avatars.FirstOrDefaultAsync(avt => avt.AssociatedRecordId == associtedID).ConfigureAwait(false);
            return avatar!;
        }

        public async Task<int> Save(Avatar avatar)
        {
            await _context.Avatars.AddAsync(avatar);
            return await _context.SaveChangesAsync().ConfigureAwait(false);
        }

    }
}
