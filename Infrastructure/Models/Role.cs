using System;
using System.Collections.Generic;

namespace Infrastructure.Models;

public partial class Role
{
    public int RoleId { get; set; }

    public string RoleName { get; set; } = null!;

    public int NumberPermissions { get; set; }

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
