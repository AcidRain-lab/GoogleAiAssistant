using System;
using System.Collections.Generic;

namespace DAL.Models;

public partial class Client
{
    public Guid Id { get; set; }

    public DateTime? CreatedDateTime { get; set; }

    public string? Email { get; set; }

    public Guid? UserId { get; set; }

    public string? Phone { get; set; }

    public bool? IsActive { get; set; }

    public bool IsIndividual { get; set; }

    public virtual ICollection<BankAccount> BankAccounts { get; set; } = new List<BankAccount>();
}
