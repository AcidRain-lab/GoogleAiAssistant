using System;
using System.Collections.Generic;

namespace DAL.Models;

public partial class BankAccount
{
    public Guid Id { get; set; }

    public string AccountNumber { get; set; } = null!;

    public string AccountName { get; set; } = null!;

    public int BankAccountTypeId { get; set; }

    public Guid ClientId { get; set; }

    public DateOnly OpenedDate { get; set; }

    public DateOnly? ClosedDate { get; set; }

    public Guid? CreatedBy { get; set; }

    public string? ContractTerms { get; set; }

    public decimal Balance { get; set; }

    public int BankCurrencyId { get; set; }

    public bool IsFop { get; set; }

    public virtual ICollection<BankAccountTransaction> BankAccountTransactions { get; set; } = new List<BankAccountTransaction>();

    public virtual BankAccountType BankAccountType { get; set; } = null!;

    public virtual ICollection<BankCard> BankCards { get; set; } = new List<BankCard>();

    public virtual BankCurrency BankCurrency { get; set; } = null!;

    public virtual Client Client { get; set; } = null!;
}
