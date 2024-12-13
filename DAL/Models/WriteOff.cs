using System;
using System.Collections.Generic;

namespace DAL.Models;

public partial class WriteOff
{
    public Guid Id { get; set; }

    public decimal? Count { get; set; }

    public Guid ProductId { get; set; }

    public decimal? Brutto { get; set; }

    public int? WriteOffTypeId { get; set; }

    public Guid? StockId { get; set; }

    public DateTime? Date { get; set; }

    public string? Description { get; set; }

    public Guid? UserId { get; set; }

    public virtual WriteOffType? WriteOffType { get; set; }
}
