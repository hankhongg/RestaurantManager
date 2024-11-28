using System;
using System.Collections.Generic;

namespace RestaurantManager.Models;

public partial class FinancialHistory
{
    public int FinId { get; set; }

    public DateTime FinDate { get; set; }

    public string? Description { get; set; }

    public string? Type { get; set; }

    public decimal Amount { get; set; }

    public int? ReferenceId { get; set; }

    public string? ReferenceType { get; set; }
}
