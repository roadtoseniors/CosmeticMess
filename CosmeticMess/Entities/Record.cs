using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CosmeticMess.Entities;

public partial class Record
{
    public int Id { get; set; }
    public int? ClientId { get; set; }
    
    public int MasterId { get; set; }
    public int ServiceTypeId { get; set; }

    public DateTime Date { get; set; }
    [JsonIgnore]
    public int? PaymentId { get; set; }

    public string? Comment { get; set; }
    
    public int StatusId { get; set; }

    public virtual User? Client { get; set; } 

    public virtual User Master { get; set; } = null!;

    public virtual PaymentType? Payment { get; set; }

    public virtual ServiceType ServiceType { get; set; } = null!;

    public virtual RecordStatus Status { get; set; } = null!;
}
