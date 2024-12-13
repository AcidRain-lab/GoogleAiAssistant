using System;
using System.Collections.Generic;

namespace DAL.Models;

public partial class State
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Location> Locations { get; set; } = new List<Location>();
}
