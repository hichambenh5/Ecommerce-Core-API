using Infrastructure.Models;
using Infrastructure.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Service
{
    public class ProductService:IProductService
    {
        private readonly IProductRepository _repo;
        public ProductService(IProductRepository repo)
        {
            _repo = repo;
        }
        public async Task<IEnumerable<Product>> GetAllProductsAsync()
            => await _repo.GetAllProductsAsync();
        public async Task<Product?> GetProductByIdAsync(int id)
            => await _repo.GetProductByIdAsync(id);
        public async Task<int> CreateProductAsync(Product product)
        {
            if(await _repo.ExistsProductAsync(product.ProductName))
            {
                throw new Exception($"Product with ID {product.ProductName} already exists.");
            }
            return await _repo.AddProductAsync(product);
        }
        public async Task<bool> UpdateProductAsync(Product product)
        {
            return await _repo.UpdateProductAsync(product);
        }
        public async Task<bool> DeleteProductAsync(int id)
        {
            return await _repo.DeleteProductAsync(id);
        }
        public async Task<IEnumerable<Product>> GetProductsByCategoryAsync(int categoryId)
            =>  await _repo.GetProductsByCategoryIdAsync(categoryId);
        public async Task<Product?> GetProductDetailsAsync(int id)
            => await _repo.GetProductDetailsByIdAsync(id);
        public async Task<IEnumerable<Product>> GetLatestProductsAsync(int count)
        => await _repo.GetLatestProductsAsync(count);

        public async Task<IEnumerable< Product>> SearchProductsAsync(string searchTerm)
            => await _repo.SearchProductsAsync(searchTerm);
    }
}
