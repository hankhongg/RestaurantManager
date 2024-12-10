using System;
using System.Collections.Generic;

namespace RestaurantManager.Models;

public partial class Ingredient
{
    public int IngreId { get; set; }

    public string IngreCode { get; set; } = null!;

    public string IngreName { get; set; } = null!;

    public double InstockKg { get; set; }

    public bool Isdeleted { get; set; }

    public decimal? IngrePrice { get; set; }

    public virtual ICollection<Recipe> Recipes { get; set; } = new List<Recipe>();

    public virtual ICollection<StockinDetailsIngre> StockinDetailsIngres { get; set; } = new List<StockinDetailsIngre>();
}
