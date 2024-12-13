using System;

namespace DAL.Models;

public partial class ShopingList
{
    public Guid Id { get; set; }
    public Guid ShopingId { get; set; }
    public Guid ProductId { get; set; }
    public decimal Quantity { get; set; }
    public bool IsBought { get; set; } // Новое поле: куплен ли продукт

    public virtual Product Product { get; set; } = null!;
    public virtual Shoping Shoping { get; set; } = null!;
}
