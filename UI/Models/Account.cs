using System;
using System.Collections.Generic;

namespace RestaurantManager.Models;

public partial class Account
{
    public string AccUsername { get; set; } = null!;

    public string AccPassword { get; set; } = null!;

    public string? AccEmail { get; set; }

    public string AccDisplayname { get; set; } = null!;

    public string AccGender { get; set; } = null!;

    public DateOnly AccBday { get; set; }

    public string AccAddress { get; set; } = null!;

    public string AccPhone { get; set; } = null!;

    public int? RoleId { get; set; }

    public bool Isdeleted { get; set; }

    public virtual AccountRole? Role { get; set; }
}
