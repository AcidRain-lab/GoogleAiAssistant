using System;
using System.Collections.Generic;

namespace DAL.Models;

public partial class CuisineType
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Dish> Dishes { get; set; } = new List<Dish>();
}
