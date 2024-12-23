using System;
using System.Collections.Generic;

namespace DAL.Models;

public partial class BankAccountTransaction
{
    public Guid Id { get; set; }

    public Guid BankAccountId { get; set; }

    public DateTime TransactionDate { get; set; }

    public int TransactionTypeId { get; set; }

    public int TransactionSourceTypeId { get; set; }

    public double Amount { get; set; }

    public double BalanceAfterTransaction { get; set; }

    public Guid? FromClientId { get; set; }

    public string? Notes { get; set; }

    public string? Iban { get; set; }

    public string? Mfo { get; set; }

    public Guid? BankCardId { get; set; }

    public virtual BankAccount BankAccount { get; set; } = null!;

    public virtual TransactionSourceType TransactionSourceType { get; set; } = null!;

    public virtual TransactionType TransactionType { get; set; } = null!;
}
