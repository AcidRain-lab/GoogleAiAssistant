using System;
using System.Collections.Generic;

namespace DAL.Models;

public partial class BankCard
{
    public Guid Id { get; set; }

    public Guid BankAccountId { get; set; }

    public string CardNumber { get; set; } = null!;

    public string CardHolderName { get; set; } = null!;

    public DateOnly ExpirationDate { get; set; }

    public string PinCode { get; set; } = null!;

    public int CardTypeId { get; set; }

    public bool IsActive { get; set; }

    public virtual BankAccount BankAccount { get; set; } = null!;

    public virtual CardType CardType { get; set; } = null!;
}
