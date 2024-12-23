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

    public DateTime? CreatedDateTime { get; set; }

    public int? PreferredLanguageId { get; set; }

    public virtual ICollection<Message> Messages { get; set; } = new List<Message>();

    public virtual Language? PreferredLanguage { get; set; }

    public virtual Role Role { get; set; } = null!;
}
