using System;
using System.Collections.Generic;

namespace DAL.Models;

public partial class TradeTemplateLabor
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public Guid AssociatedRecordId { get; set; }

    public string Text { get; set; } = null!;

    public Guid? OwnerId { get; set; }

    public DateTime? CreatedDateTime { get; set; }

    public decimal? Quantity { get; set; }

    public int? MeasurementTypeId { get; set; }

    public decimal? Waste { get; set; }

    public decimal? Measurement { get; set; }

    public decimal? CostUnit { get; set; }

    public decimal? PriceUnit { get; set; }

    public bool? UsePriceUnit { get; set; }

    public virtual TradeTemplate AssociatedRecord { get; set; } = null!;

    public virtual MeasurementType? MeasurementType { get; set; }
}
