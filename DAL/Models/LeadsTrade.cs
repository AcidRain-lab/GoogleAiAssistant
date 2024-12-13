using System;
using System.Collections.Generic;

namespace DAL.Models;

public partial class LeadsTrade
{
    public int Id { get; set; }

    public int TradeTypeId { get; set; }

    public Guid LeadId { get; set; }

    public virtual Lead Lead { get; set; } = null!;

    public virtual TradeType TradeType { get; set; } = null!;
}
