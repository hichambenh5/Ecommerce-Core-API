using System;
using System.Collections.Generic;

namespace Infrastructure.Models;

public partial class ProductReview
{
    public int ProductReviewsId { get; set; }

    public string? Comment { get; set; }

    public int Rating { get; set; }

    public DateTime CreatedAt { get; set; }

    public int? UserId { get; set; }

    public int? ProductId { get; set; }

    public virtual Product? Product { get; set; }

    public virtual User? User { get; set; }
}
