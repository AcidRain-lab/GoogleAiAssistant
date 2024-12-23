using System;
using System.Collections.Generic;

namespace DAL.Models;

public partial class CardType
{
    public string Name { get; set; } = null!;

    public int PaymentSystemTypeId { get; set; }

    public string? Description { get; set; }

    public string? Caption1 { get; set; }

    public string? Value1 { get; set; }

    public string? Caption2 { get; set; }

    public string? Value2 { get; set; }

    public string? Caption3 { get; set; }

    public string? Value3 { get; set; }

    public Guid Id { get; set; }

    public virtual ICollection<BankCard> BankCards { get; set; } = new List<BankCard>();

    public virtual PaymentSystemType PaymentSystemType { get; set; } = null!;
}
