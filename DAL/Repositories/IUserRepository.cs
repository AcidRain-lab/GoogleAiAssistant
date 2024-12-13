using DAL.Models;

namespace DAL.Repositories
{
    public interface IUserRepository
    {
        Task<bool> IsResetPasswordTokenValid(Guid token);
        Task<User> GetUserByResetPasswordToken(Guid token);
        Task<int> Update(User user);
        Task<List<User>> GetAll();
        Task<User> GetById(Guid id);
    }
}
