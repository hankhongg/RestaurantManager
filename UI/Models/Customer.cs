using System;
using System.Collections.Generic;

namespace RestaurantManager.Models;

public partial class Customer
{
    public int CusId { get; set; }

    public string CusCode { get; set; } = null!;

    public string CusName { get; set; } = null!;

    public string? CusAddr { get; set; }

    public string? CusPhone { get; set; }
       
    public string? CusCccd { get; set; }

    public string? CusEmail { get; set; }

    public bool Isdeleted { get; set; }

    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();

    public virtual ICollection<Receipt> Receipts { get; set; } = new List<Receipt>();
}
