using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CosmeticMess.Entities;

public partial class OrderItem
{
    public int Id { get; set; }
    [JsonIgnore]
    public int OrderId { get; set; }
    [JsonIgnore]
    public int ProductId { get; set; }

    public int Quantity { get; set; }

    public decimal PriceOrder { get; set; }

    public virtual Order Order { get; set; } = null!;

    public virtual Product Product { get; set; } = null!;
}
