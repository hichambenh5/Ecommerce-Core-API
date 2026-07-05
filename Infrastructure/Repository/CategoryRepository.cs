using Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repository
{
    public class CategoryRepository:ICategoryRepository
    {
        private readonly EcommerceDbContext _context;
       public CategoryRepository(EcommerceDbContext context)
        {
            _context = context;
        }
        public async Task <List<Category>> GetAllCategoriesAsync()
        {
            return await _context.Categories.AsNoTracking().ToListAsync();
        }
       public async Task<Category?> GetCategoryByIdAsync(int id)
        {
            return await _context.Categories.AsNoTracking().FirstOrDefaultAsync(x => x.CategoryId == id);
        }
        public async Task<int> AddCategoryAsync(Category category)
        {
           
                await _context.Categories.AddAsync(category);
                await _context.SaveChangesAsync();
                return category.CategoryId;
          
            
        }
        public async Task<bool> UpdateCategoryAsync(Category updatedDto)
        {
          
                var category = await _context.Categories.FindAsync(updatedDto.CategoryId);
                if (category == null) return false;
                MappingExtensions.PatchValues(category, updatedDto);
                await _context.SaveChangesAsync();
                return true;
          
          
        }
        public async Task<bool> DeleteCategoryAsync(int id)
        {

            var category = await _context.Categories.FindAsync(id);
            if (category == null) return false;

           
            category.IsDeleted = true;
            _context.Categories.Update(category);

            await _context.SaveChangesAsync();
            return true;

        }
        public async Task<bool> ExistsCategoryAsync(int id)
        {
            return await _context.Categories.AnyAsync(c => c.CategoryId == id);
           
        }
        public async Task<bool> ExistsCategoryAsync(string name)
        {
           return await _context.Categories.AnyAsync(c => c.CategoryName == name);
           
        }
        public async Task<Category?> GetCategoryWithProductsAsync(int id)
        {
            return await _context.Categories.Include(p => p.Products).AsNoTracking().FirstOrDefaultAsync(p => p.CategoryId == id);
        }
        public async Task<bool> RestoreCategoryAsync(int id)
        {
            var category = await _context.Categories
         .IgnoreQueryFilters()
         .FirstOrDefaultAsync(c => c.CategoryId == id);

           
            if (category == null) return false;

           
            if (category.IsDeleted == false) return false;

            category.IsDeleted = false;

            _context.Entry(category).State = EntityState.Modified;

            await _context.SaveChangesAsync();
            return true;
        }
    }
}
