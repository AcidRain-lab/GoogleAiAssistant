using System;
using System.Collections.Generic;

namespace DAL.Models;

public partial class Invoice
{
    public Guid Id { get; set; }

    public Guid EstimateId { get; set; }

    public DateTime? CreatedDateTime { get; set; }

    public Guid? OwnerId { get; set; }

    public virtual Estimate Estimate { get; set; } = null!;
}
