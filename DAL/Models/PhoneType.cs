using System;
using System.Collections.Generic;

namespace DAL.Models;

public partial class PhoneType
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Phone> Phones { get; set; } = new List<Phone>();
}
