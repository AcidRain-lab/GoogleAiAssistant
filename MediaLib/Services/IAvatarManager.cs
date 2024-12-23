using MediaLib.DTO.Avatars;
using DAL.Models;

namespace MediaLib.Services
{
    public interface IAvatarManager
    {
        Task<AvatarSummaryDTO> GetAvatarByAssociatedRecordId(Guid associtedID);
    }
}
