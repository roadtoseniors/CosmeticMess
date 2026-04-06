using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CosmeticMess.Entities;

public partial class Basket
{
    public int Id { get; set; } 
    [JsonIgnore]
    public int UserId { get; set; }
    [JsonIgnore]
    public virtual ICollection<BasketItem> BasketItems { get; set; } = new List<BasketItem>();

    public virtual User User { get; set; } = null!;
}
