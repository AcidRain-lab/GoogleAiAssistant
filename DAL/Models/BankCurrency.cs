using System;
using System.Collections.Generic;

namespace DAL.Models;

public partial class BankCurrency
{
    public int Id { get; set; }

    public string CurrencyCode { get; set; } = null!;

    public string CurrencyName { get; set; } = null!;

    public string? ShortName { get; set; }

    public string? Symbol { get; set; }

    public decimal ExchangeRate { get; set; }

    public bool IsActive { get; set; }

    public DateTime? CreatedDate { get; set; }

    public DateTime? LastUpdatedDate { get; set; }

    public virtual ICollection<BankAccount> BankAccounts { get; set; } = new List<BankAccount>();
}
