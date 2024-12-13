using System;
using System.Collections.Generic;

namespace DAL.Models;

public partial class Dish
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public int? CousineTypeId { get; set; }

    public string? RecieptText { get; set; }

    public decimal? Calories { get; set; }

    public virtual CuisineType? CousineType { get; set; }

    public virtual ICollection<DishTechnologyCard> DishTechnologyCards { get; set; } = new List<DishTechnologyCard>();

    public virtual ICollection<DishesMeal> DishesMeals { get; set; } = new List<DishesMeal>();

    public virtual ICollection<MealHistoryDetail> MealHistoryDetails { get; set; } = new List<MealHistoryDetail>();

    public virtual ICollection<MealTemplateDetail> MealTemplateDetails { get; set; } = new List<MealTemplateDetail>();
}
