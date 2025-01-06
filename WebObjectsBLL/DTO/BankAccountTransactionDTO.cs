using System;

namespace WebObjectsBLL.DTO
{
    public class BankAccountTransactionDTO
    {
        public Guid Id { get; set; }
        public DateTime TransactionDate { get; set; }
        public string TransactionType { get; set; }
        public decimal Amount { get; set; }
        public string FromClientName { get; set; }
        public string PaymentSystem { get; set; }
        public string Notes { get; set; }
        public Guid BankCardId { get; set; }
    }
}
