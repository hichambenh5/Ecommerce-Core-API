namespace Infrastructure.DTOs;

public class ProductResponseDto
{
    public int ProductId { get; set; }
    public string ProductName { get; set; } = null!;
    public string Note { get; set; } = null!;
    public int? CategoryId { get; set; }
}

public class ProductCreateDto
{
    public string ProductName { get; set; } = null!;
    public string Note { get; set; } = null!;
    public int? CategoryId { get; set; }
}

public class ProductUpdateDto
{
    public string? ProductName { get; set; }
    public string? Note { get; set; }
    public int? CategoryId { get; set; }
}
public class CategoryResponseDto
{
    public int CategoryId { get; set; }
    public string CategoryName { get; set; } = null!;
  
}
public class CategoryCreateDto
{
    public string CategoryName { get; set; } = null!;
}
public class CategoryUpdateDto
{
    public string CategoryName { get; set; } = null!;
}
public class CreateRoleDto
{
    public string RoleName { get; set; } = null!;
    public int NumberPermissions { get; set; }
}
public class UpdateRoleDto
{
    public int RoleId { get; set; }
    public string RoleName { get; set; } = null!;
    public int NumberPermissions { get; set; }
}
public class RoleDto
{
    public int RoleId { get; set; }
    public string RoleName { get; set; } = null!;
    public int NumberPermissions { get; set; }
}
public class UserRegisterDto
{
    public string UserName { get; set; } = null!;
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Password { get; set; } = null!; // كلمة السر الخام
}
public class UserUpdateDto
{
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
}
public class UserResponseDto
{
    public int UserId { get; set; }
    public string UserName { get; set; } = null!;
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public int? RoleId { get; set; }
}
public class UserLoginDto
{
    public string UserName { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}
public class ChangePasswordDto
{
    public string OldPassword { get; set; } = string.Empty;
    public string NewPassword { get; set; } = string.Empty;
}
public class OrderResponseDto
{
    public int OrderId { get; set; }
    public DateTime OrderDate { get; set; }
    public decimal TotalPrice { get; set; }
    public string OrderStatus { get; set; } = null!;
    public int? UserId { get; set; }
    public int? CouponsId { get; set; }
    public List<OrderItemResponseDto> OrderItems { get; set; } = new();
}
public class OrderItemResponseDto
{
    public int OrderItemId { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
    public int? VariantId { get; set; }
   
}
public class OrderCreateDto
{
    public decimal TotalPrice { get; set; }
    public int? UserId { get; set; }
    public int? CouponsId { get; set; }
    public List<OrderItemCreateDto> OrderItems { get; set; } = new();

}
public class OrderItemCreateDto
{
    public int VariantId { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
 
}
public class OrderUpdateDto
{
    public string? OrderStatus { get; set; } = null!;
    public decimal? TotalPrice { get; set; } 
}
public class CouponDto
{
    public int CouponsId { get; set; }
    public string Code { get; set; } = null!;
    public string DiscountType { get; set; } = null!;
    public decimal Value { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int UsageLimit { get; set; }
    public bool IsActive { get; set; }
}
public class CreateCouponDto
{
    public string Code { get; set; } = null!;
    public string DiscountType { get; set; } = null!;
    public decimal Value { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int UsageLimit { get; set; }
    public bool IsActive { get; set; }
}
public class UpdateCouponDto
{
 
    public string Code { get; set; } = null!;
    public string DiscountType { get; set; } = null!;
    public decimal Value { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int UsageLimit { get; set; }
    public bool IsActive { get; set; }
}