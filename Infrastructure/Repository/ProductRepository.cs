using Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repository
{
    public class ProductRepository:IProductRepository
    {
        private readonly EcommerceDbContext _context;
        public ProductRepository(EcommerceDbContext context)
        {
            _context = context;
        }
        public async Task<List<Product>> GetAllProductsAsync()
        {
            return await _context.Products.Where(p=>!p.IsDeleted).AsNoTracking().ToListAsync();
        }
        public async Task<Product?> GetProductByIdAsync(int id)
        {
            return await _context.Products.AsNoTracking().FirstOrDefaultAsync(p => p.ProductId == id);
        }
        public async Task<int> AddProductAsync(Product product)
        {
            
                await _context.Products.AddAsync(product);
                await _context.SaveChangesAsync();
                return product.ProductId;
           
           
        }
        public async Task<bool> UpdateProductAsync(Product updatedDto)
        {
           
                var product = await _context.Products.FindAsync(updatedDto.ProductId);
                if (product == null) return false;
                MappingExtensions.PatchValues(product, updatedDto);
                await _context.SaveChangesAsync();
                return true;
           
        }
        public async Task<bool> DeleteProductAsync(int id)
        {
            
                var product = await _context.Products.FindAsync(id);
                if (product == null) return false;
                product.IsDeleted = true;
                await _context.SaveChangesAsync();
                return true;
          
        }
        public async Task<bool> ExistsProductAsync(int id)
        {
            return await _context.Products.AnyAsync(p => p.ProductId == id);
        }
        public async Task<bool> ExistsProductAsync(string name)
        {
            return await _context.Products.AnyAsync(p => p.ProductName == name);
        }
        public async Task<List<Product>> GetProductsByCategoryIdAsync(int categoryId)
        {
            return await _context.Products.Where(p => p.CategoryId == categoryId).AsNoTracking().ToListAsync();
        }
        public async Task<Product?> GetProductDetailsByIdAsync(int id)
        {
            return  await _context.Products.Include(p => p.ProductVariants).Include(p => p.ProductReviews).Include(p=>p.ProductImages).AsNoTracking().FirstOrDefaultAsync(p => p.ProductId == id);

        }
        public async Task<List<Product>> GetLatestProductsAsync(int count)
        {
            return await _context.Products.OrderByDescending(p => p.ProductId).Take(count).AsNoTracking().ToListAsync();
        }
        public async Task<List<Product>> SearchProductsAsync(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))return new List<Product>();
            return await _context.Products.Where(p => p.ProductName.Contains(searchTerm)).AsNoTracking().ToListAsync();

            
        }
        public async Task<bool> RestoreProductAsync(int id)
        {
           
            var product = await _context.Products
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync(p => p.ProductId == id);

            if (product == null || !product.IsDeleted) return false;

            product.IsDeleted = false;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
