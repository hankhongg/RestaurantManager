using System;
using System.Collections.Generic;

namespace RestaurantManager.Models;

public partial class Booking
{
    public int BkId { get; set; }

    public string BkCode { get; set; } = null!;

    public int? EmpId { get; set; }

    public int? CusId { get; set; }

    public int? TabId { get; set; }

    public DateTime BkStime { get; set; }

    public DateTime BkOtime { get; set; }

    public byte BkStatus { get; set; }

    public bool Isdeleted { get; set; }

    public virtual Customer? Cus { get; set; }

    public virtual Employee? Emp { get; set; }

    public virtual DiningTable? Tab { get; set; }
}
