using System;
using System.Collections.Generic;

namespace Infrastructure.Models;

public partial class Coupon
{
    public int CouponsId { get; set; }

    public string Code { get; set; } = null!;

    public string DiscountType { get; set; } = null!;

    public decimal Value { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public int UsageLimit { get; set; }

    public bool IsActive { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
