using System;
using System.Collections.Generic;

namespace DAL.Models;

public partial class Job
{
    public Guid Id { get; set; }

    public string? Name { get; set; }

    public Guid? UserId { get; set; }

    public Guid ContactId { get; set; }

    public string? Notes { get; set; }

    public DateTime? CreatedDateTime { get; set; }

    public Guid OwnerId { get; set; }

    public int StatusId { get; set; }

    public Guid EstimateId { get; set; }

    public Guid? TeamId { get; set; }

    public virtual Estimate Estimate { get; set; } = null!;

    public virtual JobStatus Status { get; set; } = null!;

    public virtual Team? Team { get; set; }

    public virtual User? User { get; set; }
}
