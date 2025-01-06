using System;

namespace WebObjectsBLL.DTO
{
    public class BankAccountTransactionDTO
    {
        public Guid Id { get; set; }
        public DateTime TransactionDate { get; set; }
        public string TransactionType { get; set; } = "Expenses"; // Установлено значение по умолчанию
        public decimal Amount { get; set; }
        public string FromClientName { get; set; }
        public string PaymentSystem { get; set; } = "Visa"; // Установлено значение по умолчанию
        public string Notes { get; set; }
        public Guid BankCardId { get; set; }
        public Guid? TargetBankCardId { get; set; } // Для карты-получателя
    }


}
