using System;
using System.Collections.Generic;

namespace DAL.Models;

public partial class DishTechnologyCard
{
    public Guid Id { get; set; }

    public Guid DishId { get; set; }

    public Guid? ProductId { get; set; }

    public Guid? ChildDishId { get; set; }

    public decimal? Brutto { get; set; }

    public decimal? Netto { get; set; }

    public decimal? Calories { get; set; }

    public virtual Dish Dish { get; set; } = null!;

    public virtual Product? Product { get; set; }
}
