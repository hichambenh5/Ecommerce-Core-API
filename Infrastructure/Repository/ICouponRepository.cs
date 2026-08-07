using Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repository
{
    public interface ICouponRepository
    {
        Task<List<Coupon>> GetAllCouponsAsync();
        Task<Coupon?> GetCouponByIdAsync(int id);
        Task<int> AddCouponAsync(Coupon coupon);
        Task<bool> UpdateCouponAsync(Coupon coupon);
        Task<bool> DeleteCouponAsync(int id);
        Task<bool> ExistsCouponAsync(int id);
        Task<bool> ExistsCouponAsync(string code);
        Task<Coupon?> GetCouponWithOrdersAsync(int id);
        Task<bool> RestoreCouponAsync(int id);

       
        Task<Coupon?> GetCouponByCodeAsync(string code);
    }
}
