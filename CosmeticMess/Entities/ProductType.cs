using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CosmeticMess.Entities;

public partial class ProductType
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;
    [JsonIgnore]
    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
