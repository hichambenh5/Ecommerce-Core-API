using Infrastructure.DTOs;
using Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Service
{
   public interface IProductService
    {
        Task<IEnumerable<ProductResponseDto>> GetAllProductsAsync();
        Task<ProductResponseDto?> GetProductByIdAsync(int id);
        Task<int> CreateProductAsync(ProductCreateDto dto); 
        Task<bool> UpdateProductAsync(int id,ProductUpdateDto dto);
        Task<bool> DeleteProductAsync(int id); 
        Task<IEnumerable<ProductResponseDto>> GetProductsByCategoryAsync(int categoryId);
        Task<ProductResponseDto?> GetProductDetailsAsync(int id);
        Task<bool> ExistsProductAsync(int id);
        Task<bool> ExistsProductAsync(string name);
        Task<IEnumerable<ProductResponseDto>> GetLatestProductsAsync(int count);
        Task<IEnumerable<ProductResponseDto>> SearchProductsAsync(string searchTerm);
    }
}
