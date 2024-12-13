using System;
using System.Collections.Generic;

namespace DAL.Models;

public partial class TeamUser
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    public Guid TeamId { get; set; }

    public virtual Team Team { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
