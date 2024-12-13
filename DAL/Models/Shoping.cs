using System;
using System.Collections.Generic;

namespace DAL.Models;

public partial class Shoping
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    public DateTime ShopingDate { get; set; }

    public virtual ICollection<ShopingList> ShopingLists { get; set; } = new List<ShopingList>();

    public virtual User User { get; set; } = null!;
}
