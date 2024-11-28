using System;
using System.Collections.Generic;

namespace RestaurantManager.Models;

public partial class Employee
{
    public int EmpId { get; set; }

    public string EmpCode { get; set; } = null!;

    public string EmpName { get; set; } = null!;

    public string? EmpAddr { get; set; }

    public string EmpPhone { get; set; } = null!;

    public string? EmpCccd { get; set; }

    public string? EmpRole { get; set; }

    public decimal EmpSalary { get; set; }

    public bool Isdeleted { get; set; }

    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();

    public virtual ICollection<Receipt> Receipts { get; set; } = new List<Receipt>();
}
