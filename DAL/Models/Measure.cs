﻿using System;
using System.Collections.Generic;

namespace DAL.Models;

public partial class Measure
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
