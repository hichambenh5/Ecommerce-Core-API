using Infrastructure.DTOs;
using Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Service
{
    public interface IImageService
    {
        Task<IEnumerable<ImageResponseDto>> GetAllImagesAsync();
        Task<ImageResponseDto?> GetImageByIdAsync(int id);
        Task<IEnumerable<ImageResponseDto>> GetImagesByProductIdAsync(int productId);
        Task<int> CreateImageAsync(ImageCreateDto image);
        Task AddRangeImagesAsync(List<ImageCreateDto> images);
        Task<bool> UpdateImageAsync(int id, ImageUpdateDto dto);
        Task<bool> DeleteImageAsync(int id);
        Task<bool> DeleteImagesByProductIdAsync(int productId);
    }
}
