using System;
using System.Collections.Generic;

namespace DAL.Models;

public partial class Team
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public DateTime CreatedDateTime { get; set; }

    public Guid? OwnerId { get; set; }

    public Guid MainUserId { get; set; }

    public virtual ICollection<Job> Jobs { get; set; } = new List<Job>();

    public virtual User MainUser { get; set; } = null!;

    public virtual ICollection<TeamUser> TeamUsers { get; set; } = new List<TeamUser>();
}
