using System;
using System.Collections.Generic;

namespace DAL.Models;

public partial class EstimateSection
{
    public Guid Id { get; set; }

    public Guid EstimateId { get; set; }

    public string Name { get; set; } = null!;

    public virtual Estimate Estimate { get; set; } = null!;

    public virtual ICollection<SectionsItem> SectionsItems { get; set; } = new List<SectionsItem>();
}
