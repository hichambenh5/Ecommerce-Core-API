using Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repository
{
    public class ProductRepository
    {
        private readonly EcommerceDbContext _context;
        public ProductRepository(EcommerceDbContext context)
        {
            _context = context;
        }
        public async Task<List<Product>> GetAllProductsAsync()
        {
            return await _context.Products.AsNoTracking().ToListAsync();
        }
        public async Task<Product?> GetProductByIdAsync(int id)
        {
            return await _context.Products.AsNoTracking().FirstOrDefaultAsync(p => p.ProductId == id);
        }
        public async Task<int> AddProductAsync(Product product)
        {
            try
            {
                await _context.Products.AddAsync(product);
                await _context.SaveChangesAsync();
                return product.ProductId;
            }catch(Exception e)
            {
                return 0;
            }
           
        }
        public async Task<bool> UpdateProductAsync(Product updatedDto)
        {
            try
            {
                var product = await _context.Products.FindAsync(updatedDto.ProductId);
                if (product == null) return false;
                MappingExtensions.PatchValues(product, updatedDto);
                await _context.SaveChangesAsync();
                return true;
            }catch(Exception e)
            {
                return false;
            }
        }
        public async Task<bool> DeleteProductAsync(int id)
        {
            try
            {
                var product = await _context.Products.FindAsync(id);
                if (product == null) return false;
                product.IsDeleted = true;
                await _context.SaveChangesAsync();
                return true;
            }
           catch(Exception ex)
            {
                return false;
            }
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
    }
}
