using System;
using System.Collections.Generic;

namespace DAL.Models;

public partial class Deposit
{
    public Guid Id { get; set; }

    public Guid ClientId { get; set; }

    public Guid DepositTypeId { get; set; }

    public decimal DepositAmount { get; set; }

    public string Currency { get; set; } = null!;

    public decimal InterestRate { get; set; }

    public DateOnly StartDate { get; set; }

    public DateOnly MaturityDate { get; set; }

    public string? Status { get; set; }

    public virtual Client Client { get; set; } = null!;
}
