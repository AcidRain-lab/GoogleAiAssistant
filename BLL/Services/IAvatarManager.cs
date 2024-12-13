using BLL.DTO.Avatars;
using DAL.Models;

namespace BLL.Services
{
    public interface IAvatarManager
    {
        Task<AvatarSummaryDTO> GetAvatarByAssociatedRecordId(Guid associtedID);
    }
}
