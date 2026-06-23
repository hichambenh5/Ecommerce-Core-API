using System;
using System.Collections.Generic;

namespace Infrastructure.Models;

public partial class Address
{
    public int AddressId { get; set; }

    public string City { get; set; } = null!;

    public string StreetAddress { get; set; } = null!;

    public string PostalCode { get; set; } = null!;

    public int? UserId { get; set; }

    public virtual User? User { get; set; }
}
