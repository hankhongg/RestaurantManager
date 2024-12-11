using System;
using System.Collections.Generic;

namespace RestaurantManager.Models;

public partial class StockinDetailsIngre
{
    public int StoId { get; set; }

    public int IngreId { get; set; }

    public double QuantityKg { get; set; }

    public decimal Cprice { get; set; }

    public decimal? TotalCprice { get; set; }

    public virtual Ingredient Ingre { get; set; } = null!;

    public virtual Stockin Sto { get; set; } = null!;
}
