using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CosmeticMess.Entities;

public partial class BasketItem
{
    public int Id { get; set; }
    [JsonIgnore]
    public int BasketId { get; set; }
    [JsonIgnore]
    public int ProductId { get; set; }

    public int Quantity { get; set; }

    public virtual Basket Basket { get; set; } = null!;

    public virtual Product Product { get; set; } = null!;
}
