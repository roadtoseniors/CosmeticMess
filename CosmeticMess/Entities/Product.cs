using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CosmeticMess.Entities;

public partial class Product
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public decimal Price { get; set; }

    public string? Description { get; set; }

    public int DiscountPercent { get; set; }
    [JsonIgnore]
    public int ManufacturerId { get; set; }
    [JsonIgnore]
    public int ProductTypeId { get; set; }

    public decimal? Rating { get; set; }

    public bool IsFrozen { get; set; }
    [JsonIgnore]
    public virtual ICollection<BasketItem> BasketItems { get; set; } = new List<BasketItem>();

    public virtual Manufacturer Manufacturer { get; set; } = null!;
    [JsonIgnore]
    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    public virtual ProductType ProductType { get; set; } = null!;
    
    [JsonIgnore]
    public string CardBackground => DiscountPercent > 15 ? "#fff0c0" : "#ffffff";

    [JsonIgnore]
    public bool HasDiscount => DiscountPercent > 0;
}
