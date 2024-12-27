using Microsoft.AspNetCore.Http;

namespace MediaLib.Interfaces
{
    public interface IFileEntity
    {
        Guid Id { get; set; }
        string Name { get; set; }
        string Extension { get; set; }
        byte[]? Content { get; set; }
        Guid AssociatedRecordId { get; set; }
        int ObjectTypeId { get; set; }
        bool IsPrime { get; set; }
 
        string? Base64Image { get; set; }
        IFormFile? UploadedFile { get; set; }
    }
}
