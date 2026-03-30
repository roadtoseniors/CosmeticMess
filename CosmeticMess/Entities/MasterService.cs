using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CosmeticMess.Entities;

public partial class MasterService
{
    public int Id { get; set; }
    [JsonIgnore]
    public int UserId { get; set; }
    [JsonIgnore]
    public int ServiceTypeId { get; set; }

    public virtual ServiceType ServiceType { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
