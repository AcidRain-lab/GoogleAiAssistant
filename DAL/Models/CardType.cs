using System;
using System.Collections.Generic;

namespace DAL.Models;

public partial class CardType
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int PaymentSystemTypeId { get; set; }

    public string? Description { get; set; }

    public virtual ICollection<BankCard> BankCards { get; set; } = new List<BankCard>();

    public virtual PaymentSystemType PaymentSystemType { get; set; } = null!;
}
