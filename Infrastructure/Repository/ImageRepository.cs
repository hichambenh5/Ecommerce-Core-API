using Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repository
{
   public class ImageRepository:IImageRepository
    {
        private readonly EcommerceDbContext _context;
        public ImageRepository(EcommerceDbContext context)
        {
            _context = context;
        }
        public async Task<List<Image>> GetAllImageAsync()
        {
            return await _context.Images.AsNoTracking().ToListAsync();
        }
        public async Task<Image?> GetByIdAsync(int id)
        {
            return await _context.Images.AsNoTracking().FirstOrDefaultAsync(i => i.ImageId == id);
        }
        public async Task<List<Image>> GetImagesByProductIdAsync(int productId)
        {
            return await _context.Images.Where(i=>i.ProductId==productId).AsNoTracking().ToListAsync();
        }
        public async Task<int> AddAsync(Image image)
        {
            await _context.Images.AddAsync(image);
            await _context.SaveChangesAsync();
            return image.ImageId;
        }
        public async Task AddRangeAsync(List<Image> images)
        {
            await _context.AddRangeAsync(images);
            await _context.SaveChangesAsync();
        }
        public async Task<bool> UpdateAsync(Image updateimage)
        {
            var image = await _context.Images.FindAsync(updateimage.ImageId);
            if (image == null) return false;
            MappingExtensions.PatchValues(image, updateimage);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<bool> DeleteAsync(int id)
        {
            var image = await _context.Images.FindAsync(id);
            if (image == null) return false;
            _context.Remove(image);
            await _context.SaveChangesAsync();
            return true;

        }
        public async Task<bool> DeleteByProductIdAsync(int productId)
        {
            var images = await _context.Images.Where(i => i.ProductId == productId).ToListAsync();
            if (images.Count == 0) return true;
            _context.Images.RemoveRange(images);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
