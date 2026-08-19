using Infrastructure.DTOs;
using Infrastructure.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CoreCommerce_API.Controllers
{
    [Route("api/ProductVariant")]
    [ApiController]
    public class ProductVariantApi : ControllerBase
    {
        private readonly IProductVariantService _variantService;

        public ProductVariantApi(IProductVariantService variantService)
        {
            _variantService = variantService;
        }

        [HttpGet("All", Name = "GetAllVariants")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<ProductVariantResponseDto>>> GetAllVariantsAsync()
        {
            var variants = await _variantService.GetAllVariantsAsync();
            if (variants == null || !variants.Any())
            {
                return NotFound("No variants found");
            }
            return Ok(variants);
        }

        [HttpGet("{id}", Name = "GetVariantById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ProductVariantResponseDto>> GetVariantByIdAsync(int id)
        {
            if (id < 1)
            {
                return BadRequest($"Invalid id: {id}");
            }

            var variant = await _variantService.GetVariantByIdAsync(id);
            if (variant == null)
            {
                return NotFound($"Variant with id {id} not found");
            }

            return Ok(variant);
        }

        [HttpGet("Product/{productId}", Name = "GetVariantsByProductId")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<ProductVariantResponseDto>>> GetVariantsByProductIdAsync(int productId)
        {
            if (productId < 1)
            {
                return BadRequest("Invalid product id");
            }

            var variants = await _variantService.GetVariantsByProductIdAsync(productId);
            if (variants == null || !variants.Any())
            {
                return NotFound($"No variants found for product id {productId}");
            }

            return Ok(variants);
        }

        [HttpPost(Name = "AddVariantAsync")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ProductVariantResponseDto>> AddVariantAsync(ProductVariantCreateDto dto)
        {
            if (dto == null || dto.Price < 0 || dto.Quantity < 0 || dto.ProductId < 1)
            {
                return BadRequest("Invalid variant data");
            }

            var variantId = await _variantService.AddVariantAsync(dto);
            if (variantId <= 0)
            {
                return BadRequest("Error creating variant");
            }

            var createdVariant = await _variantService.GetVariantByIdAsync(variantId);
            return CreatedAtRoute("GetVariantById", new { id = variantId }, createdVariant);
        }

        [HttpPut("{id}", Name = "UpdateVariantAsync")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> UpdateVariantAsync(int id, ProductVariantUpdateDto dto)
        {
            if (id < 1 || dto == null)
            {
                return BadRequest("Invalid data");
            }

            var success = await _variantService.UpdateVariantAsync(id, dto);
            if (!success)
            {
                return NotFound($"Variant with id {id} not found or update failed");
            }

            return Ok($"Variant with id {id} has been updated successfully");
        }

        [HttpDelete("{id}", Name = "DeleteVariantAsync")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeleteVariantAsync(int id)
        {
            if (id < 1)
            {
                return BadRequest("Invalid id");
            }

            var success = await _variantService.DeleteVariantAsync(id);
            if (!success)
            {
                return NotFound($"Variant with id {id} not found");
            }

            return Ok($"Variant with id {id} has been deleted");
        }
    }
}
