using System;
using System.Collections.Generic;

namespace DAL.Models;

public partial class TradeTemplate
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public Guid TradeId { get; set; }

    public string Text { get; set; } = null!;

    public Guid? OwnerId { get; set; }

    public DateTime? CreatedDateTime { get; set; }

    public virtual Trade Trade { get; set; } = null!;

    public virtual ICollection<TradeTemplateLabor> TradeTemplateLabors { get; set; } = new List<TradeTemplateLabor>();

    public virtual ICollection<TradeTemplateMaterial> TradeTemplateMaterials { get; set; } = new List<TradeTemplateMaterial>();
}
