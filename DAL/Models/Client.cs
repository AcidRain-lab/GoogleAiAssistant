using System;
using System.Collections.Generic;

namespace DAL.Models;

public partial class Client
{
    public Guid Id { get; set; }

    public DateTime CreatedDateTime { get; set; }

    public string Email { get; set; } = null!;

    public Guid? UserId { get; set; }

    public string? Phone { get; set; }

    public bool? IsActive { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string? Gender { get; set; }

    public DateOnly? BirthDate { get; set; }

    public string PassportData { get; set; } = null!;

    public string TaxId { get; set; } = null!;

    public virtual ICollection<BankAccount> BankAccounts { get; set; } = new List<BankAccount>();

    public virtual ICollection<Cashback> Cashbacks { get; set; } = new List<Cashback>();

    public virtual ICollection<Credit> Credits { get; set; } = new List<Credit>();

    public virtual ICollection<Deposit> Deposits { get; set; } = new List<Deposit>();

    public virtual ICollection<RegularPayment> RegularPayments { get; set; } = new List<RegularPayment>();
}
