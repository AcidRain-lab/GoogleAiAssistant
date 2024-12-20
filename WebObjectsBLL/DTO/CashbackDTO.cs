using System;

namespace WebObjectsBLL.DTO
{
    public class CashbackDTO
    {
        public Guid Id { get; set; }
        public Guid ClientId { get; set; }
        public string Category { get; set; } = null!;
        public decimal Amount { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
