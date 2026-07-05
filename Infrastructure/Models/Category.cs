using System;
using System.Collections.Generic;

namespace Infrastructure.Models;

public partial class Category
{
    public int CategoryId { get; set; }

    public string CategoryName { get; set; } = null!;
    public bool IsDeleted { get; set; } = false;

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
