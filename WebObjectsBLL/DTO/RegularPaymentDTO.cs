using System;

namespace WebObjectsBLL.DTO
{
    public class RegularPaymentDTO
    {
        public Guid Id { get; set; }
        public Guid ClientId { get; set; }
        public string PaymentDetails { get; set; } = null!;
        public string PaymentType { get; set; } = null!;
        public string Frequency { get; set; } = null!;
        public DateTime NextPaymentDate { get; set; }
    }
}
