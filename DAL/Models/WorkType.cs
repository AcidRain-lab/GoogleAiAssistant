using System;
using System.Collections.Generic;

namespace DAL.Models;

public partial class WorkType
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public virtual ICollection<Estimate> Estimates { get; set; } = new List<Estimate>();

    public virtual ICollection<Lead> Leads { get; set; } = new List<Lead>();
}
