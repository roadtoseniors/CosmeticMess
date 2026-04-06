using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CosmeticMess.Entities;

public partial class OrderStatus
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;
    [JsonIgnore]
    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
