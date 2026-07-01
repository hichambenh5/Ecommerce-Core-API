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