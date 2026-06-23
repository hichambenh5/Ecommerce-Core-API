using System;
using System.Collections.Generic;

namespace Infrastructure.Models;

public partial class Order
{
    public int OrderId { get; set; }

    public DateTime OrderDate { get; set; }

    public decimal TotalPrice { get; set; }

    public string OrderStatus { get; set; } = null!;

    public int? UserId { get; set; }

    public int? CouponsId { get; set; }

    public virtual Coupon? Coupons { get; set; }

    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

    public virtual User? User { get; set; }
}
