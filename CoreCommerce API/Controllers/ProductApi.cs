using Infrastructure.DTOs;
using Infrastructure.Models;
using Infrastructure.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace CoreCommerce_API.Controllers
{
    [Route("api/Product")]
    [ApiController]
    public class ProductApi : ControllerBase

    {
        private readonly IProductService _productService;


        public ProductApi(IProductService productService)
        {
            _productService = productService;
        }
        [HttpGet("All", Name = "GetAllProducts")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public async Task<ActionResult<IEnumerable<ProductResponseDto>>> GetAllProductsAsync()
        {
            var productlist = await _productService.GetAllProductsAsync();
            if (productlist == null || !productlist.Any())
            {
                return NotFound("product not found");
            }
            return Ok(productlist);
        }
        [HttpGet("{id}", Name = "GetProductByIdAsync")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ProductResponseDto>> GetProductByIdAsync(int id)
        {
            if (id < 1)
            {
                return BadRequest($"Not Actepted id: {id}");
            }
            var product = await _productService.GetProductByIdAsync(id);
            if (product == null)
            {
                return NotFound($"product with id {id} Notfound");
            }
            return Ok(product);
        }
        [HttpPost(Name = "CreateProductAsync")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ProductCreateDto>> CreateProductAsync(ProductCreateDto dto)
        {
            if (dto == null || string.IsNullOrEmpty(dto.ProductName) || dto.CategoryId < 0)
            {
                return BadRequest("invalid product data");
            }
            var createdProduct = await _productService.CreateProductAsync(dto);
            if (createdProduct <= 0)
            {
                return BadRequest("Error creating product");
            }
            var response = new ProductResponseDto
            {
                ProductId = createdProduct,
                ProductName = dto.ProductName,
                Note = dto.Note,
                CategoryId = dto.CategoryId
            };
            return CreatedAtRoute("GetProductByIdAsync", new { id = createdProduct }, response);
        }
        [HttpDelete("{id}", Name = "DeleteProductAsync")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeleteProductAsync(int id)
        {
            if (id < 1)
            {
                return BadRequest("invalid Product data");
            }
            if (await _productService.DeleteProductAsync(id))
            {
                return Ok($"product with id {id} has been deleted");
            }
            else
            {
                return NotFound($"product with id {id} not found,no rows deleted");
            }
        }
        [HttpPut("{id}",Name = "UpdateProductAsync")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ProductUpdateDto>> UpdateProductAsync(int id, ProductUpdateDto dto)
        {
            if(id<1 || dto==null|| string.IsNullOrEmpty(dto.ProductName)|| string.IsNullOrEmpty(dto.Note)|| dto.CategoryId < 0)
            {
                return BadRequest("invalid Product data");
            }
            var product = await _productService.GetProductByIdAsync(id);
            if (product == null)
            {
                return NotFound($"product with id {id} not found");
            }
            product.ProductName = dto.ProductName;
            product.Note = dto.Note;
            product.CategoryId = dto.CategoryId;
            if (await _productService.UpdateProductAsync(id, dto))
            {
                return Ok(dto);
            }
            else
            {
                return StatusCode(500, "update error");
            }
        }
        [HttpGet("GetProductsByCategory", Name = "GetProductsByCategoryAsync")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<ProductResponseDto>>> GetProductsByCategoryAsync(int categoryId)
        {
            if (categoryId < 1)
            {
                return BadRequest("invalid data");
            }
            var ProductsByCategory = await _productService.GetProductsByCategoryAsync(categoryId);
            if(ProductsByCategory==null|| !ProductsByCategory.Any())
            {
                return NotFound("product not found");
            }
            return Ok(ProductsByCategory);
        }
        [HttpGet("ProductDetails",Name = "GetProductDetailsAsync")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ProductResponseDto?>> GetProductDetailsAsync(int id)
        {
            if (id < 1)
            {
                return BadRequest("invalid product data");
            }
            var ProductDetails = await _productService.GetProductDetailsAsync(id);
            if (ProductDetails == null)
            {
                return NotFound("product not found");
            }
            return Ok(ProductDetails);
        }
        [HttpHead("{id}",Name = "ExistsById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
       
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> ExistsProductAsync(int id)
        {
            var exist = await _productService.ExistsProductAsync(id);
            if (exist)
            {
                return Ok();
            }
            else
            {
                return NotFound();
            }
        }
        [HttpHead("name/{name}", Name = "ExistsByName")]
        [ProducesResponseType(StatusCodes.Status200OK)]

        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> ExistsProductByNameAsync(string name)
        {
            var exist = await _productService.ExistsProductAsync(name);
            if (exist)
            {
                return Ok();
            }
            else
            {
                return NotFound();
            }
        }
        [HttpGet("Latest/{count}",Name = "GetLatestProductsAsync")]
        [ProducesResponseType(StatusCodes.Status200OK)]
       
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<ProductResponseDto>>> GetLatestProductsAsync(int count)
        {
            var LatestProduct = await _productService.GetLatestProductsAsync(count);
            if (LatestProduct == null || !LatestProduct.Any())
            {
                return NotFound("product not found");
            }
            return Ok(LatestProduct);
        }
        [HttpGet("Search/{searchTerm}", Name = "SearchProductsAsync")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<ProductResponseDto>>> SearchProductsAsync(string searchTerm)
        {
            if (string.IsNullOrEmpty(searchTerm)) {
                return BadRequest("invalid data");
            }
            var Products = await _productService.SearchProductsAsync(searchTerm);
            if (Products == null || !Products.Any())
            {
                return NotFound("product not found");

            }
            return Ok(Products);
        }
        [HttpPatch("restore/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> RestoreProductAsync(int id)
        {
            var result = await _productService.RestoreProductAsync(id);
            return result ? Ok("Product restored.") : BadRequest("Product not found or already active.");
        }
    }
}
