using System;
using System.Collections.Generic;

namespace CosmeticMess.Entities;

public partial class BasketItem
{
    public int Id { get; set; }

    public int BasketId { get; set; }

    public int ProductId { get; set; }

    public int Quantity { get; set; }

    public virtual Basket Basket { get; set; } = null!;

    public virtual Product Product { get; set; } = null!;
}
