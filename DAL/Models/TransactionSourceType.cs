using System;
using System.Collections.Generic;

namespace DAL.Models;

public partial class TransactionSourceType
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public virtual ICollection<BankAccountTransaction> BankAccountTransactions { get; set; } = new List<BankAccountTransaction>();
}
