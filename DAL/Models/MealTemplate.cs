using System;
using System.Collections.Generic;

namespace DAL.Models;

public partial class MealTemplate
{
    public Guid Id { get; set; }

    public int? MealTypeId { get; set; }

    public string? Name { get; set; }

    public virtual ICollection<MealTemplateDetail> MealTemplateDetails { get; set; } = new List<MealTemplateDetail>();

    public virtual MealType? MealType { get; set; }
}
