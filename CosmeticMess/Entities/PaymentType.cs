using System;
using System.Collections.Generic;

namespace CosmeticMess.Entities;

public partial class PaymentType
{
    public int Id { get; set; }

    public string Payment { get; set; } = null!;

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual ICollection<Record> Records { get; set; } = new List<Record>();
}
