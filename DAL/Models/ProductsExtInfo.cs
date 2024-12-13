using System;
using System.Collections.Generic;

namespace DAL.Models;

public partial class ProductsExtInfo
{
    public Guid Id { get; set; }

    public Guid ProductId { get; set; }

    public Guid? ManufacturerId { get; set; }

    public Guid? SupplierId { get; set; }

    public decimal? Price { get; set; }

    public decimal? Brutto { get; set; }

    public virtual Product Product { get; set; } = null!;
}
