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
    public class ImageService:IImageService
    {
        private readonly IImageRepository _repo;

        public ImageService(IImageRepository repo)
        {
            _repo = repo;
        }
        private ImageResponseDto MapToImageDto(Image image)
        {
            return new ImageResponseDto
            {
                ImageId = image.ImageId,
                ImageUrl = image.ImageUrl,
                ProductId = image.ProductId
            };
        }

        public async Task<IEnumerable<ImageResponseDto>> GetAllImagesAsync()
        {
            var images = await _repo.GetAllImageAsync();
            return images.Select(MapToImageDto);
        }
        public async Task<ImageResponseDto?> GetImageByIdAsync(int id)
        {
            var image = await _repo.GetByIdAsync(id);
            return image == null ? null : MapToImageDto(image);
        }
        public async Task<IEnumerable<ImageResponseDto>> GetImagesByProductIdAsync(int productId)
        {
            var images = await _repo.GetImagesByProductIdAsync(productId);
            return images.Select(MapToImageDto);
        }
        public async Task<int> CreateImageAsync(ImageCreateDto dto)
        {
            try
            {
                var image = new Image
                {
                    ImageUrl = dto.ImageUrl,
                    ProductId = dto.ProductId
                };
               return await _repo.AddAsync(image);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while saving the Image to the database. Please try again later.", ex);
            }
        }
        public async Task AddRangeImagesAsync(List<ImageCreateDto> dtos)
        {
            try
            {
                var images = dtos.Select(dto => new Image { ImageUrl = dto.ImageUrl, ProductId = dto.ProductId }).ToList();
                await _repo.AddRangeAsync(images);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while saving the images range to the database.", ex);
            }
        }
        public async Task<bool> UpdateImageAsync(int id, ImageUpdateDto dto)
        {
            try
            {
                var image = await _repo.GetByIdAsync(id);
                if (image == null) return false;

                MappingExtensions.PatchValues(image, dto);

                return await _repo.UpdateAsync(image);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while updating Image with ID {id}.", ex);
            }
        }
        public async Task<bool> DeleteImageAsync(int id)
        {
            try
            {
                return await _repo.DeleteAsync(id);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while deleting Image with ID {id}.", ex);
            }
        }
        public async Task<bool> DeleteImagesByProductIdAsync(int productId)
        {
            try
            {
                return await _repo.DeleteByProductIdAsync(productId);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while deleting images for Product ID {productId}.", ex);
            }
        }
    }
}
