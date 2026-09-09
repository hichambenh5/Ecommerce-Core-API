using Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repository
{
    public interface IImageRepository
    {
        Task<List<Image>> GetAllImageAsync();

        Task<Image?> GetByIdAsync(int id);

        
        Task<List<Image>> GetImagesByProductIdAsync(int productId);

        Task<int> AddAsync(Image image);

     
        Task AddRangeAsync(List<Image> images);

        Task<bool> UpdateAsync(Image image);

        Task<bool> DeleteAsync(int id);

       
        Task<bool> DeleteByProductIdAsync(int productId);

      
    }
}
