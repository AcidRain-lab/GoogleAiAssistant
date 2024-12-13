

using DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories.Implementations
{
    public class UserRepository : IUserRepository
    {
        private readonly CrmContext _context;
        public UserRepository(CrmContext context)
        {
            _context = context;
        }
        public async Task<bool> IsResetPasswordTokenValid(Guid token) => await _context.Users.AnyAsync(x => x.ResetPasswordToken == token).ConfigureAwait(false);
        public async Task<User> GetUserByResetPasswordToken(Guid token)
        {
            var user = await _context.Users.Where(x => x.ResetPasswordToken == token).FirstOrDefaultAsync().ConfigureAwait(false);
            return user!;
        }

        public async Task<int> Update(User user)
        {
            _context.Users.Update(user);
            int result = await _context.SaveChangesAsync().ConfigureAwait(false);
            return result;
        }

        public async Task<List<User>> GetAll()
        {
            return await _context.Users.ToListAsync().ConfigureAwait(false);
        }
        public async Task<User> GetById(Guid id)
        {
            var user = await _context.Users.Where(x => x.Id == id).FirstOrDefaultAsync().ConfigureAwait(false);
            return user!;
        }
    }
}
