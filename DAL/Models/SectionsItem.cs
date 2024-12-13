using System;
using System.Collections.Generic;

namespace DAL.Models;

public partial class SectionsItem
{
    public Guid Id { get; set; }

    public Guid? TradeTemplateId { get; set; }

    public Guid EstimateSectionId { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<EstimateLabor> EstimateLabors { get; set; } = new List<EstimateLabor>();

    public virtual ICollection<EstimateMaterial> EstimateMaterials { get; set; } = new List<EstimateMaterial>();

    public virtual EstimateSection EstimateSection { get; set; } = null!;
}
