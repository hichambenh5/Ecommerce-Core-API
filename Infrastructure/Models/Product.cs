using System;
using System.Collections.Generic;

namespace Infrastructure.Models;

public partial class Product
{
    public int ProductId { get; set; }

    public string ProductName { get; set; } = null!;

    public string Note { get; set; } = null!;

    public int? CategoryId { get; set; }
    public bool IsDeleted { get; set; } = false;

    public virtual Category? Category { get; set; }

    public virtual ICollection<ProductReview> ProductReviews { get; set; } = new List<ProductReview>();

    public virtual ICollection<ProductVariant> ProductVariants { get; set; } = new List<ProductVariant>();
    public virtual ICollection<Image> ProductImages { get; set; } = new List<Image>();
}
