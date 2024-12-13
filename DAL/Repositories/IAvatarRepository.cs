
using DAL.Models;

namespace DAL.Repositories
{
    public interface IAvatarRepository
    {
        Task<Avatar> GetAvatarByAssociatedRecordId(Guid associtedID);
        Task<int> Save(Avatar avatar);
        Task<List<Avatar>> GetAll();
    }
}
