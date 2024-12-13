using System;
using System.Collections.Generic;

namespace DAL.Models;

public partial class EstimateMaterial
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public Guid AssociatedRecordId { get; set; }

    public string Text { get; set; } = null!;

    public Guid? OwnerId { get; set; }

    public DateTime? CreatedDateTime { get; set; }

    public decimal? Quantity { get; set; }

    public int? MeasurementTypeId { get; set; }

    public decimal? Cost { get; set; }

    public string? CatalogImportCode { get; set; }

    public int? CatalogTypeId { get; set; }

    public virtual SectionsItem AssociatedRecord { get; set; } = null!;

    public virtual CatalogType? CatalogType { get; set; }
}
