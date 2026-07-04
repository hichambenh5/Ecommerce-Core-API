using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Models;

public partial class Image
{
    [Key]
    public int ImageId { get; set; }

    public string ImageUrl { get; set; } = null!;

    public int? ProductId { get; set; }

    public virtual Product? Product { get; set; }
}
