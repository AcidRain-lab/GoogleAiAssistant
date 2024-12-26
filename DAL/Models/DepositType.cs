using System;
using System.Collections.Generic;

namespace DAL.Models;

public partial class DepositType
{
    public Guid Id { get; set; }

    public string DepositName { get; set; } = null!;

    public decimal MinimumAmount { get; set; }

    public decimal? MaximumAmount { get; set; }

    public string? AdditionalConditions { get; set; }

    public virtual ICollection<DepositTerm> DepositTerms { get; set; } = new List<DepositTerm>();
}
