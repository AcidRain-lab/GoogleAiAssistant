using System;

namespace WebObjectsBLL.DTO
{
    public class DepositDTO
    {
        public Guid Id { get; set; }
        public Guid ClientId { get; set; }
        public decimal DepositAmount { get; set; }
        public string Currency { get; set; } = null!;
        public double InterestRate { get; set; }
        public DateTime MaturityDate { get; set; }
    }
}
