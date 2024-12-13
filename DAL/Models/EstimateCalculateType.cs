using System;
using System.Collections.Generic;

namespace DAL.Models;

public partial class EstimateCalculateType
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public virtual ICollection<Estimate> Estimates { get; set; } = new List<Estimate>();
}
