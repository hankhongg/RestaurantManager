using System;
using System.Collections.Generic;

namespace RestaurantManager.Models;

public partial class ReceiptDetail
{
    public int RecId { get; set; }

    public int ItemId { get; set; }

    public int Quantity { get; set; }

    public decimal Price { get; set; }

    public virtual MenuItem Item { get; set; } = null!;

    public virtual Receipt Rec { get; set; } = null!;
}
