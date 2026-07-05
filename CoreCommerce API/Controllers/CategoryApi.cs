using Infrastructure.DTOs;
using Infrastructure.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CoreCommerce_API.Controllers
{
    [Route("api/Category")]
    [ApiController]
    public class CategoryApi : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        public CategoryApi(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }
        [HttpGet("All", Name = "GetAllCategories")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<CategoryResponseDto>>> GetAllCategoriesAsync()
        {
            var categorylist = await _categoryService.GetAllCategoriesAsync();
            if(categorylist == null || !categorylist.Any())
            {
                return NotFound("categories not found");
            }
            return Ok(categorylist);
        }
        [HttpGet("{id}", Name = "GetCategoryById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<CategoryResponseDto>> GetCategoryByIdAsync(int id)
        {
            if (id < 1)
            {
                return BadRequest($"Not Actepted id: {id}");
            }
            var category = await _categoryService.GetCategoryByIdAsync(id);
            if (category == null)
            {
                return NotFound($"category with id {id} Notfound");
            }
            return Ok(category);
        }
        [HttpPost(Name = "CreateCategoryAsync")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<CategoryCreateDto>> CreateCategoryAsync(CategoryCreateDto dto)
        {
            if(dto==null || string.IsNullOrEmpty(dto.CategoryName))
            {
                return BadRequest("invalid category data");
            }
            var categorycreate = await _categoryService.CreateCategoryAsync(dto);
            if (categorycreate <= 0)
            {
                return BadRequest("Error creating category");
            }
            var response = new CategoryCreateDto
            {
                CategoryName = dto.CategoryName
            };
            return CreatedAtRoute("GetCategoryByIdAsync", new { id = categorycreate }, response);
        }
        [HttpDelete("{id}", Name = "DeleteCategoryAsync")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeleteCategoryAsync(int id)
        {
            if (id < 1)
            {
                return BadRequest("invalid category data");
            }
            if(await _categoryService.DeleteCategoryAsync(id))
            {
                return Ok($"category with id {id} has been deleted");
            }
            else
            {
                return NotFound($"category with id {id} not found,no rows deleted");
            }

        }
        [HttpPut("{id}", Name = "UpdateCategoryAsync")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<CategoryUpdateDto>> UpdateCategoryAsync(int id, CategoryUpdateDto dto)
        {
            if(id<1 || dto==null || string.IsNullOrEmpty(dto.CategoryName))
            {
                return BadRequest("invalid category data");
            }
            var category = await _categoryService.GetCategoryByIdAsync(id);
            if (category == null)
            {
                return NotFound($"category with id {id} not found");
            }
            category.CategoryName = dto.CategoryName;
            if(await _categoryService.UpdateCategoryAsync(id, dto))
            {
                return Ok(dto);
            }
            else
            {
                return StatusCode(500, "update errer");
            }
        }
        [HttpHead("{id}", Name = "ExistsCategoryById")]
        [ProducesResponseType(StatusCodes.Status200OK)]

        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> ExistsCategoryAsync(int id)
        {
            var exist = await _categoryService.ExistsCategoryAsync(id);
            if (exist)
            {
                return Ok();
            }
            else
            {
                return NotFound();
            }
        }
        [HttpHead("name/{name}", Name = "ExistsCategoryByName")]
        [ProducesResponseType(StatusCodes.Status200OK)]

        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> ExistsCategoryAsync(string name)
        {
            var exist = await _categoryService.ExistsCategoryAsync(name);
            if (exist)
            {
                return Ok();
            }
            else
            {
                return NotFound();
            }
        }
        [HttpGet("CategoryWithProducts", Name = "GetCategoryWithProductsAsync")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<CategoryResponseDto?>> GetCategoryWithProductsAsync(int id)
        {
            if (id < 1)
            {
                return BadRequest("invalid category data");
            }
            var CategoryWithProducts = await _categoryService.GetCategoryWithProductsAsync(id);
            if (CategoryWithProducts == null)
            {
                return NotFound("category not found");
            }
            return Ok(CategoryWithProducts);
        }
        [HttpPatch("restore/{id}", Name = "RestoreCategory")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> RestoreCategoryAsync(int id)
        {
            var success = await _categoryService.RestoreCategoryAsync(id);
            if (!success)
            {
                return NotFound($"Category with id {id} not found or is already active.");
            }
            return Ok(new { Message = "Category restored successfully." });
        }
    }
}
