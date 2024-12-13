
using DAL.Models;

namespace BLL.DTO.MediaData
{
    public class MediaDataDTO
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Extension { get; set; } = string.Empty;

        public byte[]? Content { get; set; }

        public Guid AssociatedRecordId { get; set; }

        public bool IsPrime { get; set; }

        public int ObjectTypeId { get; set; }

        public Guid? OwnerId { get; set; }

        public DateTime? CreatedDateTime { get; set; }

    }
}
