using System;
using System.Collections.Generic;

namespace RestaurantManager.Models;

public partial class MenuItem
{
    public int ItemId { get; set; }

    public string ItemCode { get; set; } = null!;

    public string ItemType { get; set; } = null!;

    public string ItemName { get; set; } = null!;

    public string? ItemImg { get; set; }

    public decimal ItemCprice { get; set; }

    public decimal ItemSprice { get; set; }

    public bool IsAvailable { get; set; }

    public bool Isdeleted { get; set; }

    public virtual ICollection<ReceiptDetail> ReceiptDetails { get; set; } = new List<ReceiptDetail>();

    public virtual ICollection<Recipe> Recipes { get; set; } = new List<Recipe>();

    public virtual ICollection<StockinDetailsDrinkOther> StockinDetailsDrinkOthers { get; set; } = new List<StockinDetailsDrinkOther>();
}
