using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CosmeticMess.Entities;

public partial class ServiceType
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public decimal Price { get; set; }
    [JsonIgnore]
    public virtual ICollection<MasterService> MasterServices { get; set; } = new List<MasterService>();
    [JsonIgnore]
    public virtual ICollection<Record> Records { get; set; } = new List<Record>();
}
