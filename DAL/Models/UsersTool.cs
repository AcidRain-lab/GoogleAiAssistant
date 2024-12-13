using System;
using System.Collections.Generic;

namespace DAL.Models;

public partial class UsersTool
{
    public int Id { get; set; }

    public int ToolId { get; set; }

    public Guid UserId { get; set; }

    public virtual Tool Tool { get; set; } = null!;
}
