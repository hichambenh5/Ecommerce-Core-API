using Infrastructure.DTOs;
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
        private ProductResponseDto MapToProductDto(Product product)
        {
            return new ProductResponseDto
            {
                ProductId = product.ProductId,
                ProductName = product.ProductName,
                Note = product.Note,
                CategoryId = product.CategoryId
            };
        }
        public async Task<IEnumerable<ProductResponseDto>> GetAllProductsAsync()
        {
            var product = await _repo.GetAllProductsAsync();
            return product.Select( MapToProductDto);
        }
      
          
        public async Task<ProductResponseDto?> GetProductByIdAsync(int id)
        {
            var product = await _repo.GetProductByIdAsync(id);
            if (product == null) return null;
            return MapToProductDto(product);
        }
           
        public async Task<int> CreateProductAsync(ProductCreateDto dto)
        {
            if(await _repo.ExistsProductAsync(dto.ProductName))
            {
                throw new Exception($"Product with ID {dto.ProductName} already exists.");

            }
            try
            {
                var product = new Product
                {
                    ProductName = dto.ProductName,
                    Note = dto.Note,
                    CategoryId = dto.CategoryId
                };
                return await _repo.AddProductAsync(product);
            }catch(Exception ex)
            {
                throw new Exception("An error occurred while saving the product to the database. Please try again later.", ex);
            }
          
        }
        public async Task<bool> UpdateProductAsync(int id,ProductUpdateDto dto)
        {
            try
            {
                var product = await _repo.GetProductByIdAsync(id);
                if (product == null) return false;
                MappingExtensions.PatchValues(product, dto);
                return await _repo.UpdateProductAsync(product);
            }catch(Exception ex)
            {
                throw new Exception($"An error occurred while updating product with ID {id}.", ex);
            }
          
        }
        public async Task<bool> DeleteProductAsync(int id)
        {
            try
            {
                return await _repo.DeleteProductAsync(id);
            }catch(Exception ex)
            {
                throw new Exception($"An error occurred while deleting product with ID {id}.", ex);
            }
        }
        
        public async Task<IEnumerable<ProductResponseDto>> GetProductsByCategoryAsync(int categoryId)
        {
            var product = await _repo.GetProductsByCategoryIdAsync(categoryId);
            return product.Select(MapToProductDto);
        }
        public async Task<ProductResponseDto?> GetProductDetailsAsync(int id)
        {
            var product = await _repo.GetProductDetailsByIdAsync(id);
            if (product == null) return null;
            return MapToProductDto(product);
        }
        public  async Task<bool> ExistsProductAsync(int id) => await _repo.ExistsProductAsync(id);
        public async Task<bool> ExistsProductAsync(string name) => await _repo.ExistsProductAsync(name);
        public async Task<IEnumerable<ProductResponseDto>> GetLatestProductsAsync(int count)
        {
            var product = await _repo.GetLatestProductsAsync(count);
            return product.Select(MapToProductDto);

        }

        public async Task<IEnumerable< ProductResponseDto>> SearchProductsAsync(string searchTerm)
        {
            var product = await _repo.SearchProductsAsync(searchTerm);
            return product.Select(MapToProductDto);

        }
        public async Task<bool> RestoreProductAsync(int id)
        {
            try
            {
                return await _repo.RestoreProductAsync(id);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
