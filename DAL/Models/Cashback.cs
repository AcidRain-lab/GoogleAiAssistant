using System;
using System.Collections.Generic;

namespace DAL.Models;

public partial class Cashback
{
    public Guid Id { get; set; }

    public Guid ClientId { get; set; }

    public string Category { get; set; } = null!;

    public decimal Amount { get; set; }

    public DateTime? CreatedDate { get; set; }

    public virtual Client Client { get; set; } = null!;
}
