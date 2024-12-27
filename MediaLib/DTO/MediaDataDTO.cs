using MediaLib.Interfaces;
using Microsoft.AspNetCore.Http;

namespace MediaLib.DTO
{
    public class MediaDataDTO : IFileEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Extension { get; set; } = string.Empty;
        public byte[]? Content { get; set; }
        public Guid AssociatedRecordId { get; set; }
        public int ObjectTypeId { get; set; }
        public bool IsPrime { get; set; }
        public string? Base64Image { get; set; }
        public IFormFile? UploadedFile { get; set; }
    }
}
