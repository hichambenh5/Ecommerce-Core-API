using Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repository
{
    public class CouponRepository:ICouponRepository
    {
        private readonly  EcommerceDbContext _Context;
        public CouponRepository(EcommerceDbContext context)
        {
            _Context = context;
        }
        public async Task<List<Coupon>> GetAllCouponsAsync()
        {
            return await _Context.Coupons.AsNoTracking().ToListAsync();
        }
        public async Task<Coupon?> GetCouponByIdAsync(int id)
        {
            return await _Context.Coupons.AsNoTracking().FirstOrDefaultAsync(c => c.CouponsId == id);
        }
        public async Task<int> AddCouponAsync(Coupon coupon)
        {
            await _Context.Coupons.AddAsync(coupon);
            await _Context.SaveChangesAsync();
            return coupon.CouponsId;
        }
        public async Task<bool> UpdateCouponAsync(Coupon updatecoupon)
        {
            var coupon = await _Context.Coupons.FindAsync(updatecoupon.CouponsId);
            if (coupon == null) return false;
            MappingExtensions.PatchValues(coupon, updatecoupon);
            await _Context.SaveChangesAsync();
            return true;
        }
        public async Task<bool> DeleteCouponAsync(int id)
        {
            var coupon = await _Context.Coupons.FindAsync(id);
            if (coupon == null) return false;
            _Context.Coupons.Remove(coupon);
            await _Context.SaveChangesAsync();
            return true;
        }
        public async Task<bool> ExistsCouponAsync(int id)
        {
            return await _Context.Coupons.AnyAsync(c => c.CouponsId == id);
        }
        public async Task<bool> ExistsCouponAsync(string code)
        {
            return await _Context.Coupons.AnyAsync(c => c.Code==code);
        }
        public async Task<Coupon?> GetCouponWithOrdersAsync(int id)
        {
            return await _Context.Coupons.Include(o => o.Orders).AsNoTracking().FirstOrDefaultAsync(o=> o.CouponsId == id);
        }
        public async Task<bool> RestoreCouponAsync(int id)
        {
            var coupon = await _Context.Coupons.IgnoreQueryFilters().FirstOrDefaultAsync(c => c.CouponsId == id);
            if (coupon == null) return false;
            if (coupon.IsActive == true) return false;
            coupon.IsActive = true;
            _Context.Entry(coupon).State = EntityState.Modified;

            await _Context.SaveChangesAsync();
            return true;
        }
        public async Task<Coupon?> GetCouponByCodeAsync(string code)
        {
            return await _Context.Coupons.AsNoTracking().FirstOrDefaultAsync(c => c.Code==code);
        }
    }
}
