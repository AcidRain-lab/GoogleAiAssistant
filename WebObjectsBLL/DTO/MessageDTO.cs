using System;

namespace WebObjectsBLL.DTO
{
    public class MessageDTO
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Content { get; set; } = null!;
        public DateTime? CreatedDate { get; set; }
    }
}
