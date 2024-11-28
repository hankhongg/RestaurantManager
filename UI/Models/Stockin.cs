using System;
using System.Collections.Generic;

namespace RestaurantManager.Models;

public partial class Stockin
{
    public int StoId { get; set; }

    public string StoCode { get; set; } = null!;

    public DateTime StoDate { get; set; }

    public decimal StoPrice { get; set; }

    public virtual ICollection<StockinDetailsDrinkOther> StockinDetailsDrinkOthers { get; set; } = new List<StockinDetailsDrinkOther>();

    public virtual ICollection<StockinDetailsIngre> StockinDetailsIngres { get; set; } = new List<StockinDetailsIngre>();
}
