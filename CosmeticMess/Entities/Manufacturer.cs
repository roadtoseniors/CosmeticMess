using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CosmeticMess.Entities;

public partial class Manufacturer
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;
    [JsonIgnore]
    public int CountryId { get; set; }

    public virtual Country Country { get; set; } = null!;
    [JsonIgnore]
    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
