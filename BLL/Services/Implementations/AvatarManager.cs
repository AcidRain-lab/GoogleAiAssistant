using BLL.DTO.Avatars;
using DAL.Models;
using DAL.Repositories;

namespace BLL.Services.Implementations
{
    public class AvatarManager : IAvatarManager
    {
        private readonly IAvatarRepository _avatarRepository;
        public AvatarManager(IAvatarRepository avatarRepository)
        {
            _avatarRepository = avatarRepository;
        }

        public async Task<AvatarSummaryDTO> GetAvatarByAssociatedRecordId(Guid associtedID)
        {
            var avatar = await _avatarRepository.GetAvatarByAssociatedRecordId(associtedID);
            AvatarSummaryDTO avatarSummary = new();
            if (avatar != null)
            {
                avatarSummary.Extension = avatar.Extension;
                avatarSummary.AssociatedRecordId = associtedID;
                avatarSummary.Content = avatar.Content;
                avatarSummary.Name = avatar.Name;
            }
            return avatarSummary;
        }
    }
}
