using System;
using System.Collections.Generic;

namespace DAL.Models;

public partial class OrdersMealsHistory
{
    public Guid Id { get; set; }

    public int? Number { get; set; }

    public string? Description { get; set; }

    public virtual ICollection<MealHistoryDetail> MealHistoryDetails { get; set; } = new List<MealHistoryDetail>();

    public virtual ICollection<OrdersMeal> OrdersMeals { get; set; } = new List<OrdersMeal>();
}
