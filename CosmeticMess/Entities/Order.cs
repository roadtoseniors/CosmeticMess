using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CosmeticMess.Entities;

public partial class Order
{
    public int Id { get; set; }
    [JsonIgnore]
    public int UserId { get; set; }

    public DateTime Date { get; set; }

    public DateOnly DeliveryDate { get; set; }
    [JsonIgnore]
    public int PaymentId { get; set; }
    [JsonIgnore]
    public int StatusId { get; set; }
    [JsonIgnore]
    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    public virtual PaymentType Payment { get; set; } = null!;

    public virtual OrderStatus Status { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
