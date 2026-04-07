using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CosmeticMess.Entities;

public partial class MasterService
{
    public int Id { get; set; }
    
    public int UserId { get; set; }
    
    public int ServiceTypeId { get; set; }

    public virtual ServiceType ServiceType { get; set; } = null!;
    [JsonIgnore]
    public virtual User User { get; set; } = null!;
}
