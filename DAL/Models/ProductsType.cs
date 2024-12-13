using System;
using System.Collections.Generic;

namespace DAL.Models;

public partial class ProductsType
{
    public Guid Id { get; set; }

    public string? Name { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
