using System;
using System.Collections.Generic;

namespace Infrastructure.Models;

public partial class User
{
    public int UserId { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public string UserName { get; set; } = null!;

    public int? RoleId { get; set; }

    public virtual ICollection<Address> Addresses { get; set; } = new List<Address>();

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual ICollection<Phone> Phones { get; set; } = new List<Phone>();

    public virtual ICollection<ProductReview> ProductReviews { get; set; } = new List<ProductReview>();

    public virtual Role? Role { get; set; }

    public virtual ICollection<ShoppingCart> ShoppingCarts { get; set; } = new List<ShoppingCart>();
}
