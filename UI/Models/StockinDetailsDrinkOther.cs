using System;
using System.Collections.Generic;

namespace RestaurantManager.Models;

public partial class StockinDetailsDrinkOther
{
    public int StoId { get; set; }

    public int ItemId { get; set; }

    public int QuantityUnits { get; set; }

    public decimal Cprice { get; set; }

    public virtual MenuItem Item { get; set; } = null!;

    public virtual Stockin Sto { get; set; } = null!;
}
