using System;
using System.Collections.Generic;

namespace DAL.Models;

public partial class OrdersProduct
{
    public Guid Id { get; set; }

    public Guid ProductId { get; set; }

    public decimal? Brutto { get; set; }

    public Guid OrderId { get; set; }

    public decimal? Price { get; set; }

    public decimal? CustomerPrice { get; set; }

    public Guid? TaxId { get; set; }

    public virtual OrdersMeal Order { get; set; } = null!;
}
