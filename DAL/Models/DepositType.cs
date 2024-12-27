using System;
using System.Collections.Generic;

namespace DAL.Models;

public partial class DepositType
{
    public Guid Id { get; set; }

    public string DepositName { get; set; } = null!;

    public string? Description { get; set; }

    public virtual ICollection<DepositTerm> DepositTerms { get; set; } = new List<DepositTerm>();

    public virtual ICollection<Deposit> Deposits { get; set; } = new List<Deposit>();
}
