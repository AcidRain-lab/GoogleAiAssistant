using MediaLib.DTO;
using System;

namespace WebObjectsBLL.DTO
{
    public class CardTypeDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public int PaymentSystemTypeId { get; set; }
        public string? Description { get; set; }
        public string? Caption1 { get; set; }
        public string? Value1 { get; set; }
        public string? Caption2 { get; set; }
        public string? Value2 { get; set; }
        public string? Caption3 { get; set; }
        public string? Value3 { get; set; }
        public string PaymentSystemTypeName { get; set; } = null!;

        // Добавлено свойство для отображения аватара
        public string? AvatarBase64 { get; set; }
        public List<MediaDataDTO>? MediaFiles { get; set; }
    }
}
