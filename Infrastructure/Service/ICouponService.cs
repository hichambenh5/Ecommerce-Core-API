using Infrastructure.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Service
{
    public interface ICouponService
    {
        Task<IEnumerable<CouponDto>> GetAllCouponsAsync();
        Task<CouponDto?> GetCouponByIdAsync(int id);
        Task<CouponDto?> GetCouponByCodeAsync(string code);
        Task<int> CreateCouponAsync(CreateCouponDto createDto);
        Task<bool> UpdateCouponAsync(int id,UpdateCouponDto updateDto);
        Task<bool> DeleteCouponAsync(int id);
        Task<bool> RestoreCouponAsync(int id);
        Task<bool> ExistsAsync(int id);
        Task<bool> ExistsAsync(string code);
    }
}
