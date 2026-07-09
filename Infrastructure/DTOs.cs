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
