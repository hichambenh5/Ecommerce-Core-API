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
        Task<IEnumerable<Product>> GetAllProductsAsync();
        Task<Product?> GetProductByIdAsync(int id);
        Task<int> CreateProductAsync(Product product); 
        Task<bool> UpdateProductAsync(Product product);
        Task<bool> DeleteProductAsync(int id); 
        Task<IEnumerable<Product>> GetProductsByCategoryAsync(int categoryId);
        Task<Product?> GetProductDetailsAsync(int id);
        Task<IEnumerable<Product>> GetLatestProductsAsync(int count);
        Task<IEnumerable<Product>> SearchProductsAsync(string searchTerm);
    }
}
