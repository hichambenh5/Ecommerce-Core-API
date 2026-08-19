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
   public class ProductVariantService:IProductVariantService
    {
        private readonly IProductVariantRepository _variantRepository;

        public ProductVariantService(IProductVariantRepository variantRepository)
        {
            _variantRepository = variantRepository;
        }

        public async Task<IEnumerable<ProductVariantResponseDto>> GetAllVariantsAsync()
        {
            var variants = await _variantRepository.GetAllVariantsAsync();
            return variants.Select(v => new ProductVariantResponseDto
            {
                VariantId = v.VariantId,
                Quantity = v.Quantity,
                Color = v.Color,
                Size = v.Size,
                Price = v.Price,
                ProductId = v.ProductId
            });
        }

        public async Task<ProductVariantResponseDto?> GetVariantByIdAsync(int id)
        {
            var v = await _variantRepository.GetVariantByIdAsync(id);
            if (v == null) return null;

            return new ProductVariantResponseDto
            {
                VariantId = v.VariantId,
                Quantity = v.Quantity,
                Color = v.Color,
                Size = v.Size,
                Price = v.Price,
                ProductId = v.ProductId
            };
        }

        public async Task<IEnumerable<ProductVariantResponseDto>> GetVariantsByProductIdAsync(int productId)
        {
            var variants = await _variantRepository.GetVariantsByProductIdAsync(productId);
            return variants.Select(v => new ProductVariantResponseDto
            {
                VariantId = v.VariantId,
                Quantity = v.Quantity,
                Color = v.Color,
                Size = v.Size,
                Price = v.Price,
                ProductId = v.ProductId
            });
        }

        public async Task<int> AddVariantAsync(ProductVariantCreateDto createDto)
        {
            var variant = new ProductVariant
            {
                Quantity = createDto.Quantity,
                Color = createDto.Color,
                Size = createDto.Size,
                Price = createDto.Price,
                ProductId = createDto.ProductId
            };

            return await _variantRepository.AddVariantAsync(variant);
        }

        public async Task<bool> UpdateVariantAsync(int id, ProductVariantUpdateDto updateDto)
        {
            var variant = await _variantRepository.GetVariantByIdAsync(id);
            if (variant == null) return false;

            if (updateDto.Quantity.HasValue) variant.Quantity = updateDto.Quantity.Value;
            if (!string.IsNullOrEmpty(updateDto.Color)) variant.Color = updateDto.Color;
            if (!string.IsNullOrEmpty(updateDto.Size)) variant.Size = updateDto.Size;
            if (updateDto.Price.HasValue) variant.Price = updateDto.Price.Value;

            return await _variantRepository.UpdateVariantAsync(variant);
        }

        public async Task<bool> DeleteVariantAsync(int id)
        {
            return await _variantRepository.DeleteVariantAsync(id);
        }

        public async Task<bool> ExistsVariantAsync(int id)
        {
            return await _variantRepository.ExistsVariantAsync(id);
        }
    }
}
