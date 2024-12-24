using DAL.Models;
using MediaLib.DTO;
using MediaLib.Helpers;
using Microsoft.EntityFrameworkCore;

namespace MediaLib.Services
{
    public class AvatarService
    {
        private readonly BankContext _context;

        public AvatarService(BankContext context)
        {
            _context = context;
        }

        public async Task<AvatarDTO?> GetAvatarAsync(Guid associatedRecordId)
        {
            var avatar = await _context.Avatars.FirstOrDefaultAsync(m => m.AssociatedRecordId == associatedRecordId);
            if (avatar == null) return null;

            return new AvatarDTO
            {
                Id = avatar.AssociatedRecordId,
                Name = avatar.Name,
                Extension = avatar.Extension,
                Content = avatar.Content,
                AssociatedRecordId = avatar.AssociatedRecordId,
                ObjectTypeId = avatar.ObjectTypeId,
                Base64Image = avatar.Content != null ? FileHelper.ToBase64(avatar.Content) : null
            };
        }

        public async Task<bool> SetAvatarAsync(AvatarDTO model)
        {
            var avatar = await _context.Avatars.FirstOrDefaultAsync(a => a.AssociatedRecordId == model.AssociatedRecordId);

            if (model.Content == null)
            {
                return false; // Нельзя сохранить пустой аватар
            }

            // Используем FileHelper для обработки
            var optimizedContent = FileHelper.ResizeAndCompressImage(model.Content, 100, 100);

            if (avatar != null)
            {
                avatar.Name = model.Name;
                avatar.Content = optimizedContent;
                avatar.Extension = ".png";
                _context.Avatars.Update(avatar);
            }
            else
            {
                var newAvatar = new Avatar
                {
                    AssociatedRecordId = model.AssociatedRecordId,
                    Name = model.Name,
                    Extension = ".png",
                    Content = optimizedContent,
                    ObjectTypeId = model.ObjectTypeId
                };
                _context.Avatars.Add(newAvatar);
            }

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RemoveAvatarAsync(Guid associatedRecordId)
        {
            var avatar = await _context.Avatars.FirstOrDefaultAsync(a => a.AssociatedRecordId == associatedRecordId);
            if (avatar == null) return false;

            _context.Avatars.Remove(avatar);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
