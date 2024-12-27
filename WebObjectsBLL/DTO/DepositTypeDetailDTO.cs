using MediaLib.DTO;
using System;
using System.Collections.Generic;

namespace WebObjectsBLL.DTO
{
    public class DepositTypeDetailDTO
    {
        public Guid Id { get; set; }
        public string DepositName { get; set; } = string.Empty;
        public string? Description { get; set; }
        public List<DepositTermDTO> DepositTerms { get; set; } = new();

        // Новые поля для работы с медиа
        public AvatarDTO? Avatar { get; set; }
        public List<MediaDataDTO>? MediaFiles { get; set; }
        public List<DocumentsDTO>? Documents { get; set; }
    }
}
