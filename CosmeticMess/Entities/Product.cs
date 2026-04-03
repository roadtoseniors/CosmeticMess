using System;
using System.Collections.Generic;

namespace CosmeticMess.Entities;

public partial class Product
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public decimal Price { get; set; }

    public string? Description { get; set; }

    public int DiscountPercent { get; set; }

    public int ManufacturerId { get; set; }

    public int ProductTypeId { get; set; }

    public decimal? Rating { get; set; }

    public bool IsFrozen { get; set; }

    public virtual ICollection<BasketItem> BasketItems { get; set; } = new List<BasketItem>();

    public virtual Manufacturer Manufacturer { get; set; } = null!;

    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    public virtual ProductType ProductType { get; set; } = null!;
}
