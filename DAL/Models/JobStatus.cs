using System;
using System.Collections.Generic;

namespace DAL.Models;

public partial class JobStatus
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Job> Jobs { get; set; } = new List<Job>();
}
