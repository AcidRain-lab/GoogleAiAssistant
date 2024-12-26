using System;

namespace WebObjectsBLL.DTO
{
    public class BankCardDTO
    {
        public Guid Id { get; set; }
        public Guid BankAccountId { get; set; }
        public string CardNumber { get; set; } = null!;
        public string CardHolderName { get; set; } = null!;
        public DateTime ExpirationDate { get; set; }
        public string PinCode { get; set; } = null!;
        public string CardTypeName { get; set; } = null!;
        public Guid CardTypeId { get; set; } // Добавлено свойство
        public decimal CreditLimit { get; set; }
        public bool IsPrimary { get; set; }
        public bool AllowExternalTransfers { get; set; }
    }

}
