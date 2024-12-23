using DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MediaLib.DTO;


namespace MediaLib
{
    public static class MediaHelpers
    {

        public static string GetAvatarBase64(Guid associatedRecordId, BankContext context)
        {
            var mediaData = context.Avatars.FirstOrDefault(m => m.AssociatedRecordId == associatedRecordId);
            return mediaData != null ? Convert.ToBase64String(mediaData.Content) : string.Empty;
        }


        public static byte[]? GetAvatarBytes(Guid associatedRecordId, BankContext context)
        {
            var mediaData = context.Avatars.FirstOrDefault(m => m.AssociatedRecordId == associatedRecordId);
            return mediaData?.Content;
        }

        public static async Task<bool> HasAvatarAsync(Guid associatedRecordId, BankContext context)
        {
            var exists = await context.Avatars.AnyAsync(m => m.AssociatedRecordId == associatedRecordId);
            return exists;
        }


        public static async Task<string> GetAvatarBase64Async(Guid associatedRecordId, BankContext context)
        {
            var mediaData = await context.Avatars.FirstOrDefaultAsync(m => m.AssociatedRecordId == associatedRecordId);
            return mediaData != null ? Convert.ToBase64String(mediaData.Content) : string.Empty;
        }

        public static async Task<byte[]>? GetAvatarBytesAsync(Guid associatedRecordId, BankContext context)
        {
            var mediaData = await context.Avatars.FirstOrDefaultAsync(m => m.AssociatedRecordId == associatedRecordId);
            return mediaData?.Content;
        }

    }
}
