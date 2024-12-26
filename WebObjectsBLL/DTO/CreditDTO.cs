using MediaLib.DTO;
using System;

namespace WebObjectsBLL.DTO
{

    public class CreditDTO
    {
        public Guid Id { get; set; }
        public Guid ClientId { get; set; }
        public decimal CreditAmount { get; set; }
        public string Currency { get; set; } = string.Empty;
        public double InterestRate { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsActive { get; set; }
        public Guid CreditTypeId { get; set; } // Добавлено поле
    }

}