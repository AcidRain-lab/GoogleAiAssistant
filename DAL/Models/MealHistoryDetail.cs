using System;
using System.Collections.Generic;

namespace DAL.Models;

public partial class MealHistoryDetail
{
    public Guid Id { get; set; }

    public Guid MealHistoryId { get; set; }

    public Guid DishId { get; set; }

    public int? Row { get; set; }

    public int? Column { get; set; }

    public decimal? Price { get; set; }

    public decimal? CustomerPrice { get; set; }

    public Guid? TaxId { get; set; }

    public virtual Dish Dish { get; set; } = null!;

    public virtual OrdersMealsHistory MealHistory { get; set; } = null!;
}
