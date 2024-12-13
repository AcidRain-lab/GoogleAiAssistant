using System;
using System.Collections.Generic;

namespace DAL.Models;

public partial class Estimate
{
    public Guid Id { get; set; }

    public string? Name { get; set; }

    public Guid UserId { get; set; }

    public Guid ContactId { get; set; }

    public string? Notes { get; set; }

    public DateTime CreatedDateTime { get; set; }

    public Guid? OwnerId { get; set; }

    public int? StatusId { get; set; }

    public int CalculateTypeId { get; set; }

    public decimal Square { get; set; }

    public int WorkTypeId { get; set; }

    public int JobCategoryId { get; set; }

    public decimal? PriceMaterial { get; set; }

    public decimal? PriceLabor { get; set; }

    public decimal? TotalPrice { get; set; }

    public string? Text { get; set; }

    public virtual EstimateCalculateType CalculateType { get; set; } = null!;

    public virtual ICollection<EstimateSection> EstimateSections { get; set; } = new List<EstimateSection>();

    public virtual ICollection<Invoice> Invoices { get; set; } = new List<Invoice>();

    public virtual JobCategory JobCategory { get; set; } = null!;

    public virtual ICollection<Job> Jobs { get; set; } = new List<Job>();

    public virtual EstimateStatus? Status { get; set; }

    public virtual WorkType WorkType { get; set; } = null!;
}
