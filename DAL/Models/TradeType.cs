using System;
using System.Collections.Generic;

namespace DAL.Models;

public partial class TradeType
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public virtual ICollection<LeadsTrade> LeadsTrades { get; set; } = new List<LeadsTrade>();
}
