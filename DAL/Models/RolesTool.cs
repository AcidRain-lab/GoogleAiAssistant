using System;
using System.Collections.Generic;

namespace DAL.Models;

public partial class RolesTool
{
    public int Id { get; set; }

    public int RoleId { get; set; }

    public int ToolId { get; set; }

    public virtual Role Role { get; set; } = null!;

    public virtual Tool Tool { get; set; } = null!;
}
