using System;
using System.Collections.Generic;

namespace DAL.Models;

public partial class OrdersMeal
{
    public Guid Id { get; set; }

    public int? Number { get; set; }

    public DateTime? DateCreate { get; set; }

    public Guid MealHistoryId { get; set; }

    public Guid CustomerUserId { get; set; }

    public Guid ManagerUserId { get; set; }

    public string? Description { get; set; }

    public DateTime? DateDelivery { get; set; }

    public int OrderStateId { get; set; }

    public string? Invoice { get; set; }

    public virtual OrdersMealsHistory MealHistory { get; set; } = null!;

    public virtual OrdersState OrderState { get; set; } = null!;

    public virtual ICollection<OrdersProduct> OrdersProducts { get; set; } = new List<OrdersProduct>();
}
