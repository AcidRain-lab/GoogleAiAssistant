using System;
using System.Collections.Generic;

namespace DAL.Models;

public partial class MeasurementType
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<EstimateLabor> EstimateLabors { get; set; } = new List<EstimateLabor>();

    public virtual ICollection<TradeTemplateLabor> TradeTemplateLabors { get; set; } = new List<TradeTemplateLabor>();
}
