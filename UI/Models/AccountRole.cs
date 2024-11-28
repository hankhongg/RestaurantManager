using System;
using System.Collections.Generic;

namespace RestaurantManager.Models;

public partial class AccountRole
{
    public int RoleId { get; set; }

    public string? RoleName { get; set; }

    public virtual ICollection<Account> Accounts { get; set; } = new List<Account>();
}
