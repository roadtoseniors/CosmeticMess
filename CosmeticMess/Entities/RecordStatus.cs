using System;
using System.Collections.Generic;

namespace CosmeticMess.Entities;

public partial class RecordStatus
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Record> Records { get; set; } = new List<Record>();
}
