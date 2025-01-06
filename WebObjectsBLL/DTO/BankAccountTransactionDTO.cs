using System;

namespace WebObjectsBLL.DTO
{
    public class BankAccountTransactionDTO
    {
        public Guid Id { get; set; }
        public DateTime TransactionDate { get; set; }
        public string TransactionType { get; set; } = null!; // Имя типа транзакции (Income, Expenses, Transfers)
        public double Amount { get; set; }
        public double BalanceAfterTransaction { get; set; }
        public string? Notes { get; set; }
    }
}
