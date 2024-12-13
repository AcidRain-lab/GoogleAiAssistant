using System;
using System.Collections.Generic;

namespace DAL.Models;

public partial class DishesMeal
{
    public Guid Id { get; set; }

    public Guid DishId { get; set; }

    public int MealTypeId { get; set; }

    public virtual Dish Dish { get; set; } = null!;

    public virtual MealType MealType { get; set; } = null!;
}
