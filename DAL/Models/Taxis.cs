using System;
using System.Collections.Generic;

namespace DAL.Models;

public partial class Taxis
{
    public Guid Id { get; set; }

    public decimal? Price { get; set; }

    public string? Name { get; set; }
}
