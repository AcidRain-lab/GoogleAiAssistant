using System;
using System.Collections.Generic;

namespace DAL.Models;

public partial class CreditType
{
    public Guid Id { get; set; }

    public string CreditName { get; set; } = null!;

    public string CreditAmount { get; set; } = null!;

    public string CreditTerm { get; set; } = null!;

    public string? AdditionalConditions { get; set; }

    public virtual ICollection<Credit> Credits { get; set; } = new List<Credit>();
}
