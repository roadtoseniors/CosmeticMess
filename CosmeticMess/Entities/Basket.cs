using System;
using System.Collections.Generic;

namespace CosmeticMess.Entities;

public partial class Basket
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public virtual ICollection<BasketItem> BasketItems { get; set; } = new List<BasketItem>();

    public virtual User User { get; set; } = null!;
}
