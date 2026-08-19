using Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository
{
    public class ProductVariantRepository:IProductVariantRepository
    {
        private readonly EcommerceDbContext _context;

        public ProductVariantRepository(EcommerceDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ProductVariant>> GetAllVariantsAsync()
        {
            return await _context.ProductVariants.AsNoTracking().ToListAsync();
        }

        public async Task<ProductVariant?> GetVariantByIdAsync(int id)
        {
            return await _context.ProductVariants.AsNoTracking().FirstOrDefaultAsync(v => v.VariantId == id);
        }

        public async Task<IEnumerable<ProductVariant>> GetVariantsByProductIdAsync(int productId)
        {
            return await _context.ProductVariants
                .Where(v => v.ProductId == productId)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<int> AddVariantAsync(ProductVariant variant)
        {
            await _context.ProductVariants.AddAsync(variant);
            await _context.SaveChangesAsync();
            return variant.VariantId;
        }

        public async Task<bool> UpdateVariantAsync(ProductVariant variant)
        {
            _context.ProductVariants.Update(variant);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteVariantAsync(int id)
        {
            var variant = await _context.ProductVariants.FindAsync(id);
            if (variant == null) return false;

            _context.ProductVariants.Remove(variant);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> ExistsVariantAsync(int id)
        {
            return await _context.ProductVariants.AnyAsync(v => v.VariantId == id);
        }
    }
}

