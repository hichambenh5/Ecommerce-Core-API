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
    public class CouponService:ICouponService
    {
        private readonly ICouponRepository _Repo;
        public CouponService(ICouponRepository repo)
        {
            _Repo = repo;
        }
        private CouponDto MapToCouponDto(Coupon coupon)
        {
            return new CouponDto
            {
                CouponsId = coupon.CouponsId,
                Code=coupon.Code,
                DiscountType=coupon.DiscountType,
                Value=coupon.Value,
                StartDate=coupon.StartDate,
                EndDate=coupon.EndDate,
                UsageLimit=coupon.UsageLimit,
                IsActive=coupon.IsActive
            };
        }
        public async Task<IEnumerable<CouponDto>> GetAllCouponsAsync()
        {
            var coupon = await _Repo.GetAllCouponsAsync();
            return coupon.Select(MapToCouponDto);
        }
        public async Task<CouponDto?> GetCouponByIdAsync(int id)
        {
            var coupon = await _Repo.GetCouponByIdAsync(id);
            return coupon == null ? null : MapToCouponDto(coupon);
        }
        public async Task<CouponDto?> GetCouponByCodeAsync(string code)
        {
            var coupon = await _Repo.GetCouponByCodeAsync(code);
            return coupon == null ? null : MapToCouponDto(coupon);
        }
        public async Task<int> CreateCouponAsync(CreateCouponDto createDto)
        {
            try
            {
                var coupon = new Coupon { Code = createDto.Code,
                    DiscountType=createDto.DiscountType,
                    Value=createDto.Value,
                    StartDate=createDto.StartDate,
                    EndDate=createDto.EndDate,
                    UsageLimit=createDto.UsageLimit,
                    IsActive=createDto.IsActive
                };
                return await _Repo.AddCouponAsync(coupon);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while saving the Coupon to the database. Please try again later.", ex);
            }
        }
        public async Task<bool> UpdateCouponAsync(int id,UpdateCouponDto updateDto)
        {
            try
            {
                var coupon = await _Repo.GetCouponByIdAsync(id);
                if (coupon == null) return false;
               
                MappingExtensions.PatchValues(coupon, updateDto);
                return await _Repo.UpdateCouponAsync(coupon);

            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while updating Coupon with ID {id}.", ex);
            }
        }
        public async Task<bool> DeleteCouponAsync(int id)
        {
            try
            {
                return await _Repo.DeleteCouponAsync(id);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while deleting Coupon with ID {id}.", ex);
            }
        }
        public async Task<bool> RestoreCouponAsync(int id)
        {
            try
            {
                return await _Repo.RestoreCouponAsync(id);
            }catch(Exception ex)
            {
                throw;
            }
        }
        public async Task<bool> ExistsAsync(int id) => await _Repo.ExistsCouponAsync(id);
        public async Task<bool> ExistsAsync(string code) => await _Repo.ExistsCouponAsync(code);
    }
}
