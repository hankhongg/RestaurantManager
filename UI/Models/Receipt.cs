using System;
using System.Collections.Generic;

namespace RestaurantManager.Models;

public partial class Receipt
{
    public int RecId { get; set; }

    public string RecCode { get; set; } = null!;

    public int? EmpId { get; set; }

    public int? CusId { get; set; }

    public int? TabId { get; set; }

    public DateTime RecTime { get; set; }

    public decimal RecPay { get; set; }

    public bool Isdeleted { get; set; }

    public virtual Customer? Cus { get; set; }

    public virtual Employee? Emp { get; set; }

    public virtual ICollection<ReceiptDetail> ReceiptDetails { get; set; } = new List<ReceiptDetail>();

    public virtual DiningTable? Tab { get; set; }
}
