using System;
using System.Collections.Generic;

namespace RestaurantManager.Models;

public partial class Recipe
{
    public int ItemId { get; set; }

    public int IngreId { get; set; }

    public double IngreQuantityKg { get; set; }

    public virtual Ingredient Ingre { get; set; } = null!;

    public virtual MenuItem Item { get; set; } = null!;
}
