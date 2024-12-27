using MediaLib.DTO;
using System;

namespace WebObjectsBLL.DTO
{
    public class DepositTypeTableDTO
    {
        public Guid Id { get; set; }
        public string DepositName { get; set; } = string.Empty;
        public string? Description { get; set; }

        // Аватарка и первичное медиа
        public AvatarDTO? Avatar { get; set; }
        public MediaDataDTO? PrimaryMedia { get; set; }
    }
}
