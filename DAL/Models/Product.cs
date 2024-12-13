using System;
using System.Collections.Generic;

namespace DAL.Models;

public partial class Product
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public decimal? CaloriesPer100 { get; set; }

    public decimal? ProteinsPer100 { get; set; }

    public decimal? CarbsPer100 { get; set; }

    public decimal? FatsPer100 { get; set; }

    public bool? IsAllergyc { get; set; }

    public Guid? MeasureId { get; set; }

    public Guid? ProductTypeId { get; set; }

    public virtual ICollection<DishTechnologyCard> DishTechnologyCards { get; set; } = new List<DishTechnologyCard>();

    public virtual Measure? Measure { get; set; }

    public virtual ProductsType? ProductType { get; set; }

    public virtual ICollection<ProductsCookingBasket> ProductsCookingBaskets { get; set; } = new List<ProductsCookingBasket>();

    public virtual ICollection<ProductsExtInfo> ProductsExtInfos { get; set; } = new List<ProductsExtInfo>();

    public virtual ICollection<ShopingList> ShopingLists { get; set; } = new List<ShopingList>();

    public virtual ICollection<Stock> Stocks { get; set; } = new List<Stock>();
}
