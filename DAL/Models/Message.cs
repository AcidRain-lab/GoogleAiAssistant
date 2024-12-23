using System;
using System.Collections.Generic;

namespace DAL.Models;

public partial class Message
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    public string Content { get; set; } = null!;

    public DateTime? CreatedDate { get; set; }

    public virtual User User { get; set; } = null!;
}
