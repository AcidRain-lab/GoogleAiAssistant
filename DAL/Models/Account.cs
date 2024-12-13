using System;
using System.Collections.Generic;

namespace DAL.Models;

public partial class Account
{
    public Guid Id { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string? Phone { get; set; }

    public string? Address { get; set; }

    public string? City { get; set; }

    public string? State { get; set; }

    public string? Zip { get; set; }

    public string? Notes { get; set; }

    public int AccountTypeId { get; set; }

    public Guid? UserId { get; set; }

    public DateTime? CreatedDateTime { get; set; }

    public int CountryId { get; set; }

    public string? SecretWord { get; set; }

    public virtual AccountType AccountType { get; set; } = null!;

    public virtual Country Country { get; set; } = null!;

    public virtual User? User { get; set; }
}
