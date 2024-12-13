using System;
using System.Collections.Generic;

namespace DAL.Models;

public partial class Trade
{
    public Guid Id { get; set; }

    public string? Name { get; set; }

    public virtual ICollection<TradeTemplate> TradeTemplates { get; set; } = new List<TradeTemplate>();
}
