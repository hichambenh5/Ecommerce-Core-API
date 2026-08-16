using Infrastructure.DTOs;
using Infrastructure.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CoreCommerce_API.Controllers
{
    [Route("api/Coupon")]
    [ApiController]
    public class CouponApi : ControllerBase
    {
        private readonly ICouponService _couponService;

        public CouponApi(ICouponService couponService)
        {
            _couponService = couponService;
        }

        [HttpGet("All", Name = "GetAllCoupons")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<CouponDto>>> GetAllCouponsAsync()
        {
            var couponsList = await _couponService.GetAllCouponsAsync();
            if (couponsList == null || !couponsList.Any())
            {
                return NotFound("Coupons not found");
            }
            return Ok(couponsList);
        }

        [HttpGet("{id}", Name = "GetCouponById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<CouponDto>> GetCouponByIdAsync(int id)
        {
            if (id < 1)
            {
                return BadRequest($"Not Accepted id: {id}");
            }

            var coupon = await _couponService.GetCouponByIdAsync(id);
            if (coupon == null)
            {
                return NotFound($"Coupon with id {id} Not found");
            }

            return Ok(coupon);
        }

        [HttpGet("code/{code}", Name = "GetCouponByCode")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<CouponDto>> GetCouponByCodeAsync(string code)
        {
            if (string.IsNullOrEmpty(code))
            {
                return BadRequest("Invalid Coupon code");
            }

            var coupon = await _couponService.GetCouponByCodeAsync(code);
            if (coupon == null)
            {
                return NotFound($"Coupon with code '{code}' Not found");
            }

            return Ok(coupon);
        }

        [HttpPost(Name = "CreateCouponAsync")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<CreateCouponDto>> CreateCouponAsync(CreateCouponDto dto)
        {
            if (dto == null || string.IsNullOrEmpty(dto.Code) || dto.Value <= 0)
            {
                return BadRequest("Invalid Coupon Data");
            }

            var couponId = await _couponService.CreateCouponAsync(dto);
            if (couponId <= 0)
            {
                return BadRequest("Error creating Coupon");
            }

            return CreatedAtRoute("GetCouponById", new { id = couponId }, dto);
        }

        [HttpPut("{id}", Name = "UpdateCouponAsync")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> UpdateCouponAsync(int id, UpdateCouponDto dto)
        {
            if (id < 1 || dto == null)
            {
                return BadRequest("Invalid Coupon Data");
            }

            var isUpdated = await _couponService.UpdateCouponAsync(id, dto);
            if (!isUpdated)
            {
                return NotFound($"Coupon with id {id} not found or update failed");
            }

            return Ok(dto);
        }

        [HttpDelete("{id}", Name = "DeleteCouponAsync")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeleteCouponAsync(int id)
        {
            if (id < 1)
            {
                return BadRequest("Invalid Coupon ID");
            }

            if (await _couponService.DeleteCouponAsync(id))
            {
                return Ok($"Coupon with id {id} has been deleted");
            }
            else
            {
                return NotFound($"Coupon with id {id} not found, no rows deleted");
            }
        }

        [HttpPost("{id}/restore", Name = "RestoreCouponAsync")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> RestoreCouponAsync(int id)
        {
            if (id < 1)
            {
                return BadRequest("Invalid Coupon ID");
            }

            var restored = await _couponService.RestoreCouponAsync(id);
            if (restored)
            {
                return Ok($"Coupon with id {id} has been restored successfully");
            }
            else
            {
                return NotFound($"Coupon with id {id} not found or could not be restored");
            }
        }

        [HttpHead("{id}", Name = "ExistsCouponByIdAsync")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> ExistsCouponAsync(int id)
        {
            var exist = await _couponService.ExistsAsync(id);
            if (exist)
            {
                return Ok();
            }
            else
            {
                return NotFound();
            }
        }
    
}
}
