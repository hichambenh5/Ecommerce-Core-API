using Infrastructure.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Service
{
  public interface IProductVariantService
    {
        Task<IEnumerable<ProductVariantResponseDto>> GetAllVariantsAsync();
        Task<ProductVariantResponseDto?> GetVariantByIdAsync(int id);
        Task<IEnumerable<ProductVariantResponseDto>> GetVariantsByProductIdAsync(int productId);
        Task<int> AddVariantAsync(ProductVariantCreateDto createDto);
        Task<bool> UpdateVariantAsync(int id, ProductVariantUpdateDto updateDto);
        Task<bool> DeleteVariantAsync(int id);
        Task<bool> ExistsVariantAsync(int id);
    }
}
