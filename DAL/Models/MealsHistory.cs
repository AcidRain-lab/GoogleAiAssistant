using System;
using System.Collections.Generic;

namespace DAL.Models;

public partial class MealsHistory
{
    public Guid Id { get; set; }

    public DateOnly Date { get; set; }

    public Guid? MealTemplateId { get; set; }

    public string? Name { get; set; }

    public int? MealTypeId { get; set; }

    public virtual ICollection<MealHistoryDetail> MealHistoryDetails { get; set; } = new List<MealHistoryDetail>();

    public virtual ICollection<OrdersMeal> OrdersMeals { get; set; } = new List<OrdersMeal>();
}
