using System;

namespace WebObjectsBLL.DTO
{
    public class DepositTermDTO
    {
        public Guid Id { get; set; } // Уникальный идентификатор срока депозита

        public int TermMonths { get; set; } // Срок депозита в месяцах

        public decimal InterestRate { get; set; } // Процентная ставка

        public string Currency { get; set; } = null!; // Валюта депозита

        public Guid DepositTypeId { get; set; } // Идентификатор связанного типа депозита
    }
}
