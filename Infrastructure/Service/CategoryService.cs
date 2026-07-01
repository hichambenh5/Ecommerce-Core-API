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
   public class CategoryService :ICategoryService
    {
        private readonly ICategoryRepository _repo;

        public CategoryService(ICategoryRepository repo)
        {
            _repo = repo;
        }

        private CategoryResponseDto MapToCategoryDto(Category category)
        {
            return new CategoryResponseDto
            {
                CategoryId = category.CategoryId,
                CategoryName = category.CategoryName
               
            };
        }
        public async Task<IEnumerable<CategoryResponseDto>> GetAllCategoriesAsync()
        {
            var categories = await _repo.GetAllCategoriesAsync();
            return categories.Select(MapToCategoryDto);
        }
        public async Task<CategoryResponseDto?> GetCategoryByIdAsync(int id)
        {
            var category = await _repo.GetCategoryByIdAsync(id);
            return category == null ? null : MapToCategoryDto(category);
        }
        public async Task<int> CreateCategoryAsync(CategoryCreateDto dto)
        {
            try
            {
                var category = new Category { CategoryName = dto.CategoryName };
                return await _repo.AddCategoryAsync(category);
            }
            catch(Exception ex)
            {
                throw new Exception("An error occurred while saving the Category to the database. Please try again later.", ex);
            }
        }
        public async Task<bool> UpdateCategoryAsync(int id, CategoryUpdateDto dto)
        {
            try
            {
                var category = await _repo.GetCategoryByIdAsync(id);
                if (category == null) return false;
                category.CategoryName = dto.CategoryName;
                MappingExtensions.PatchValues(category, dto);
                return await _repo.UpdateCategoryAsync(category);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while updating Category with ID {id}.", ex);
            }
        }
        public async Task<bool> DeleteCategoryAsync(int id)
        {
            try
            {
                return await _repo.DeleteCategoryAsync(id);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while deleting Category with ID {id}.", ex);
            }
        }
        public async Task<bool> ExistsCategoryAsync(int id) => await _repo.ExistsCategoryAsync(id);

        public async Task<bool> ExistsCategoryAsync(string name) => await _repo.ExistsCategoryAsync(name);

        public async Task<CategoryResponseDto?> GetCategoryWithProductsAsync(int id)
        {
            var category = await _repo.GetCategoryWithProductsAsync(id);
            if (category == null) return null;

            var dto = MapToCategoryDto(category);
         
            return dto;
        }
    }
}
