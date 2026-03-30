using System;
using System.Collections.Generic;

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

    public virtual ICollection<Basket> Baskets { get; set; } = new List<Basket>();

    public virtual ICollection<MasterService> MasterServices { get; set; } = new List<MasterService>();

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual ICollection<Record> RecordClients { get; set; } = new List<Record>();

    public virtual ICollection<Record> RecordMasters { get; set; } = new List<Record>();

    public virtual Role Role { get; set; } = null!;
}
