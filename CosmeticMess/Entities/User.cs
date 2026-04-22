using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CosmeticMess.Entities;

public partial class User
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string? Phone { get; set; }
    public int RoleId { get; set; }

    public string? Login { get; set; }

    public string? Password { get; set; }

    public int? Age { get; set; }

    public bool IsFrozen { get; set; }
    [JsonIgnore]
    public virtual ICollection<Basket> Baskets { get; set; } = new List<Basket>();
    
    public virtual ICollection<MasterService> MasterServices { get; set; } = new List<MasterService>();
    [JsonIgnore]
    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
    [JsonIgnore]
    public virtual ICollection<Record> RecordClients { get; set; } = new List<Record>();
    [JsonIgnore]
    public virtual ICollection<Record> RecordMasters { get; set; } = new List<Record>();

    public virtual Role Role { get; set; } = null!;
    
    public string FrozenLabel => IsFrozen ? "Заморожен" : "Активен";
    public string FrozenColor => IsFrozen ? "#b0c4de" : "#d4edda";
}
