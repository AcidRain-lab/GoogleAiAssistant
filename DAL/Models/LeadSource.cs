using System;
using System.Collections.Generic;

namespace DAL.Models;

public partial class LeadSource
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public virtual ICollection<Lead> Leads { get; set; } = new List<Lead>();
}
