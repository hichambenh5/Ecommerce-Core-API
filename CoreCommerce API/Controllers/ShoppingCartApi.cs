using Infrastructure.DTOs;
using Infrastructure.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CoreCommerce_API.Controllers
{
    [Route("api/ShoppingCart")]
    [ApiController]
    public class ShoppingCartApi : ControllerBase
    {
        private readonly IShoppingCartService _shoppingCartService;
        public ShoppingCartApi(IShoppingCartService shoppingCartService)
        {
            _shoppingCartService = shoppingCartService;

        }
        [HttpGet("{id}",Name = "GetUserCart")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<CartItemDto>> GetUserCartAsync(int userId)
        {
            if (userId < 1)
            {
                return BadRequest($"Not Actepted id: {userId}");
            }
            var UserCart = await _shoppingCartService.GetUserCartAsync(userId);
            if (UserCart == null)
            {
                return NotFound($"UserCard with id {userId} Notfound");
            }
            return Ok(
                UserCart);
        }
        [HttpPost("CreateCart")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<AddToCartDto>>CreateCartAsync(AddToCartDto dto){ 
             if(dto.VariantId<1 || dto.Quantity <= 0)
            {
                return BadRequest("Invalid shoppingcard data");
            }
            var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
            {
                return Unauthorized("Invalid or missing user token.");
            }
            var shoppingcardid = await _shoppingCartService.CreateCartAsync(userId, dto);
            if (shoppingcardid <=0)
            {
                return BadRequest("Error creating shoppingcard");
            }
            var Usercard = await _shoppingCartService.GetUserCartAsync(userId);
            return CreatedAtRoute("GetUserCart", new { id = shoppingcardid }, Usercard);
        }
        [HttpPost("AddItem")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddItemToCart(AddToCartDto dto)
        {
            if (dto.VariantId < 1 || dto.Quantity <= 0)
            {
                return BadRequest("Invalid item data");
            }

            var userId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier).Value);

            await _shoppingCartService.AddItemToCartAsync(userId, dto);
            return Ok(new { message = "Item added/updated successfully" });
        }

        [HttpPut("UpdateQuantity/{shoppingCartId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateItemQuantity(int shoppingCartId, UpdateCartItemDto dto)
        {
            if (shoppingCartId < 1)
            {
                return BadRequest("Invalid cart item ID");
            }

            var success = await _shoppingCartService.UpdateItemQuantityAsync(shoppingCartId, dto);
            if (!success)
            {
                return NotFound($"Cart item with ID {shoppingCartId} not found");
            }

            return Ok(new { message = "Cart item updated successfully" });
        }

        [HttpDelete("RemoveItem/{shoppingCartId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> RemoveCartItem(int shoppingCartId)
        {
            if (shoppingCartId < 1)
            {
                return BadRequest("Invalid cart item ID");
            }

            // يمكنك إضافة تحقق هنا للتأكد أن العنصر موجود قبل حذفه إذا أردت
            await _shoppingCartService.RemoveCartItemAsync(shoppingCartId);
            return Ok(new { message = "Cart item removed successfully" });
        }

        [HttpDelete("ClearCart")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> ClearUserCart()
        {
            var userId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier).Value);

            await _shoppingCartService.ClearUserCartAsync(userId);
            return Ok(new { message = "Shopping cart cleared successfully" });
        }

    }
}
