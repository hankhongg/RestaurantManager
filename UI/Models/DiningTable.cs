using System;
using System.Collections.Generic;

namespace RestaurantManager.Models;

public partial class DiningTable
{
    public int TabId { get; set; }

    public byte? TabNum { get; set; }

    public bool TabStatus { get; set; }

    public bool Isdeleted { get; set; }

    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();

    public virtual ICollection<Receipt> Receipts { get; set; } = new List<Receipt>();
}
