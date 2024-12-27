using System;
using System.Collections.Generic;

namespace DAL.Models;

public partial class DepositTerm
{
    public Guid Id { get; set; }

    public Guid DepositTypeId { get; set; }

    public int TermMonths { get; set; }

    public decimal InterestRate { get; set; }

    public string Currency { get; set; } = null!;

    public virtual DepositType DepositType { get; set; } = null!;
}
