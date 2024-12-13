using System;
using System.Collections.Generic;

namespace DAL.Models;

public partial class MealType
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<DishesMeal> DishesMeals { get; set; } = new List<DishesMeal>();

    public virtual ICollection<MealTemplate> MealTemplates { get; set; } = new List<MealTemplate>();
}
