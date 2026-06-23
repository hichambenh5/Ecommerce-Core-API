using System;
using System.Collections.Generic;

namespace Infrastructure.Models;

public partial class Phone
{
    public int PhoneId { get; set; }

    public string PhoneNumber { get; set; } = null!;

    public int? UserId { get; set; }

    public virtual User? User { get; set; }
}
