using System;
using System.Collections.Generic;

namespace DAL.Models;

public partial class User
{
    public Guid Id { get; set; }

    public string UserName { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string? Phone { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public bool IsActive { get; set; }

    public int RoleId { get; set; }

    public Guid? OwnerId { get; set; }

    public DateTime? CreatedDateTime { get; set; }

    public Guid? ResetPasswordToken { get; set; }

    public string? EmailPassword { get; set; }

    public string? EmailSmtpServer { get; set; }

    public int? EmailSmtpPort { get; set; }

    public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();

    public virtual ICollection<Job> Jobs { get; set; } = new List<Job>();

    public virtual Role Role { get; set; } = null!;

    public virtual ICollection<Shoping> Shopings { get; set; } = new List<Shoping>();

    public virtual ICollection<TeamUser> TeamUsers { get; set; } = new List<TeamUser>();

    public virtual ICollection<Team> Teams { get; set; } = new List<Team>();
}
