using System;

namespace WebObjectsBLL.DTO
{
    public class BankAccountTransactionDTO
    {
        public Guid Id { get; set; }
        public DateTime TransactionDate { get; set; }
        public string TransactionType { get; set; } = "Expenses";
        public double Amount { get; set; }
        public double BalanceAfterTransaction { get; set; }
        public string FromClientName { get; set; }
        public string PaymentSystem { get; set; } = "Visa";
        public string? Notes { get; set; }
        public Guid BankCardId { get; set; }
        public Guid BankAccountId { get; set; }
        public string? Iban { get; set; }
        public string? Mfo { get; set; }

        // Добавляем свойство AccountNumber
        public string AccountNumber { get; set; } = string.Empty;
    }
}
