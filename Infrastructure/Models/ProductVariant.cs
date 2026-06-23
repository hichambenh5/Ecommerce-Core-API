using System;
using System.Collections.Generic;

namespace Infrastructure.Models;

public partial class ProductVariant
{
    public int VariantId { get; set; }

    public int Quantity { get; set; }

    public string Color { get; set; } = null!;

    public string Size { get; set; } = null!;

    public decimal Price { get; set; }

    public int? ProductId { get; set; }

    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    public virtual Product? Product { get; set; }

    public virtual ICollection<ShoppingCart> ShoppingCarts { get; set; } = new List<ShoppingCart>();
}
