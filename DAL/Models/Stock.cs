using System;
using System.Collections.Generic;

namespace DAL.Models;

public partial class Stock
{
    public Guid Id { get; set; }

    public Guid ProductId { get; set; }

    public decimal? Brutto { get; set; }

    public virtual Product Product { get; set; } = null!;

    public virtual ICollection<ProductsCookingBasket> ProductsCookingBaskets { get; set; } = new List<ProductsCookingBasket>();
}
