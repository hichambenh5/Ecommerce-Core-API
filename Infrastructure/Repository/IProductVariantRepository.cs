using Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repository
{
  public interface IProductVariantRepository
    {
        Task<IEnumerable<ProductVariant>> GetAllVariantsAsync();
        Task<ProductVariant?> GetVariantByIdAsync(int id);
        Task<IEnumerable<ProductVariant>> GetVariantsByProductIdAsync(int productId);
        Task<int> AddVariantAsync(ProductVariant variant);
        Task<bool> UpdateVariantAsync(ProductVariant variant);
        Task<bool> DeleteVariantAsync(int id);
        Task<bool> ExistsVariantAsync(int id);
    }
}
