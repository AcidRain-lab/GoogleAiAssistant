using MediaLib.DTO;
using System;

namespace WebObjectsBLL.DTO
{
    public class CardTypeTableDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string PaymentSystemTypeName { get; set; } = string.Empty;
        public AvatarDTO? Avatar { get; set; }
    }
}

