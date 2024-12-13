using System;
using System.Collections.Generic;

namespace DAL.Models;

public partial class OrdersState
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<OrdersMeal> OrdersMeals { get; set; } = new List<OrdersMeal>();
}
