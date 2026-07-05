using Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repository
{
   public interface IProductRepository
    {
        Task<List<Product>> GetAllProductsAsync();
        Task<Product?> GetProductByIdAsync(int id);
        Task<int> AddProductAsync(Product product);
        Task<bool> UpdateProductAsync(Product updatedDto);
        Task<bool> DeleteProductAsync(int id);
        Task<bool> ExistsProductAsync(int id);
        Task<bool> ExistsProductAsync(string name);
        Task<List<Product>> GetProductsByCategoryIdAsync(int categoryId);
        Task<Product?> GetProductDetailsByIdAsync(int id);
        Task<List<Product>> GetLatestProductsAsync(int count);
        Task<List<Product>> SearchProductsAsync(string searchTerm);
        Task<bool> RestoreProductAsync(int id);

    }
}
