using System;
using System.Collections.Generic;

namespace DAL.Models;

public partial class MealTemplateDetail
{
    public Guid Id { get; set; }

    public Guid MealTemplateId { get; set; }

    public Guid DishId { get; set; }

    public virtual Dish Dish { get; set; } = null!;

    public virtual MealTemplate MealTemplate { get; set; } = null!;
}
