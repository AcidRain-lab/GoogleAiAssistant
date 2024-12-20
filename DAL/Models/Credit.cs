using System;
using System.Collections.Generic;

namespace DAL.Models;

public partial class Credit
{
    public Guid Id { get; set; }

    public Guid ClientId { get; set; }

    public decimal CreditAmount { get; set; }

    public string Currency { get; set; } = null!;

    public double InterestRate { get; set; }

    public DateOnly StartDate { get; set; }

    public DateOnly EndDate { get; set; }

    public bool IsActive { get; set; }

    public virtual Client Client { get; set; } = null!;
}
