using System;
using System.Collections.Generic;

namespace DAL.Models;

public partial class Role
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public virtual ICollection<RolesTool> RolesTools { get; set; } = new List<RolesTool>();

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
