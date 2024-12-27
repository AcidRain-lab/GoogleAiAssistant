using MediaLib.DTO;
using System;
using System.Collections.Generic;

namespace WebObjectsBLL.DTO
{
    public class CardTypeDetailDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int PaymentSystemTypeId { get; set; }
        public string? Description { get; set; }
        public string? Caption1 { get; set; }
        public string? Value1 { get; set; }
        public string? Caption2 { get; set; }
        public string? Value2 { get; set; }
        public string? Caption3 { get; set; }
        public string? Value3 { get; set; }
        public string? PaymentSystemTypeName { get; set; }

        // Связанные данные
        public AvatarDTO? Avatar { get; set; }
        public List<MediaDataDTO>? MediaFiles { get; set; }
        public List<DocumentsDTO>? Documents { get; set; }
    }
}
