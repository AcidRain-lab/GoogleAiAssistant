using System;
using System.Collections.Generic;

namespace DAL.Models;

public partial class CatalogType
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<EstimateMaterial> EstimateMaterials { get; set; } = new List<EstimateMaterial>();

    public virtual ICollection<TradeTemplateMaterial> TradeTemplateMaterials { get; set; } = new List<TradeTemplateMaterial>();
}
