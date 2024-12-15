using System;
using System.Collections.Generic;

namespace DAL.Models;

public partial class PaymentSystemType
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public virtual ICollection<CardType> CardTypes { get; set; } = new List<CardType>();
}
