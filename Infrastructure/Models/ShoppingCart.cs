using System;
using System.Collections.Generic;

namespace Infrastructure.Models;

public partial class ShoppingCart
{
    public int ShoppingCartId { get; set; }

    public int Quantity { get; set; }

    public int? UserId { get; set; }

    public int? VariantId { get; set; }

    public virtual User? User { get; set; }

    public virtual ProductVariant? Variant { get; set; }
}
