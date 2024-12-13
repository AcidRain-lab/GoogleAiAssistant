using System;
using System.Collections.Generic;

namespace DAL.Models;

public partial class EstimateStatus
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Estimate> Estimates { get; set; } = new List<Estimate>();
}
