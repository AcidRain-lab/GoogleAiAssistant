using System;
using System.Collections.Generic;

namespace DAL.Models;

public partial class ProductsCookingBasket
{
    public Guid Id { get; set; }

    public Guid? ProductId { get; set; }

    public decimal? Brutto { get; set; }

    public Guid? StockId { get; set; }

    public virtual Product? Product { get; set; }

    public virtual Stock? Stock { get; set; }
}
