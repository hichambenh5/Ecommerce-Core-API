using Infrastructure.DTOs;
using Infrastructure.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace CoreCommerce_API.Controllers
{
    [Route("api/Images")]
    [ApiController]
    public class ImagesApi : ControllerBase
    {
        private readonly IImageService _imageService;

        public ImagesApi(IImageService imageService)
        {
            _imageService = imageService;
        }
        public class ImageUploadRequestDto
        {
            [Required]
            public int ProductId { get; set; }

            [Required]
            public IFormFile File { get; set; }
        }
        [HttpGet("All", Name = "GetAllImages")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAllImages()
        {
            var images = await _imageService.GetAllImagesAsync();
            if (images == null)
            {
                return NotFound("images not found");
            }
            return Ok(images);
        }

        [HttpGet("{id}",Name = "GetImageById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetImageById(int id)
        {
            if (id < 1)
            {
                return BadRequest($"Not Actepted id: {id}");
            }
            var image = await _imageService.GetImageByIdAsync(id);
            if (image == null) return NotFound(new { message = $"Image with ID {id} not found." });
            return Ok(image);
        }

        [HttpGet("product/{productId}", Name = "GetImagesByProductId")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetImagesByProductId(int productId)
        {
            if (productId < 1)
            {
                return BadRequest($"Not Actepted id: {productId}");
            }
            var images = await _imageService.GetImagesByProductIdAsync(productId);
            if (images == null)
            {
                return NotFound(new { message = $"Images not found." });
            }
            return Ok(images); 
        }

        [HttpPost(Name = "CreateImage")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> CreateImage([FromBody] ImageCreateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var imageId = await _imageService.CreateImageAsync(dto);
            return CreatedAtAction(nameof(GetImageById), new { id = imageId }, new { id = imageId, message = "Image created successfully." });
        }
        [HttpPost("range", Name = "AddRangeImages")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> AddRangeImages([FromBody] List<ImageCreateDto> dtos)
        {
            if (dtos == null || !dtos.Any()) return BadRequest(new { message = "The images list cannot be empty." });
            if (!ModelState.IsValid) return BadRequest(ModelState);

            await _imageService.AddRangeImagesAsync(dtos);
            return Ok(new { message = "Images added successfully." });
        }

        [HttpPut("{id}",Name = "UpdateImage")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateImage(int id, [FromBody] ImageUpdateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var updated = await _imageService.UpdateImageAsync(id, dto);
            if (!updated) return NotFound(new { message = $"Image with ID {id} not found for update." });

            return Ok(new { message = "Image updated successfully." });
        }

        [HttpDelete("{id}",Name = "DeleteImage")]
        [ProducesResponseType(StatusCodes.Status200OK)]
       
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteImage(int id)
        {
            var deleted = await _imageService.DeleteImageAsync(id);
            if (!deleted) return NotFound(new { message = $"Image with ID {id} not found for deletion." });

            return Ok(new { message = "Image deleted successfully." });
        }

        [HttpDelete("product{productId}",Name = "DeleteImagesByProductId")]
        [ProducesResponseType(StatusCodes.Status200OK)]

        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteImagesByProductId(int productId)
        {
            var deleted = await _imageService.DeleteImagesByProductIdAsync(productId);
            if (!deleted) return NotFound(new { message = $"No images found for Product ID {productId}." });

            return Ok(new { message = "All images for the product deleted successfully." });
        }
        [HttpPost("upload", Name = "UploadImage")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UploadImage([FromForm] ImageUploadRequestDto request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            if (request.File == null || request.File.Length == 0)
            {
                return BadRequest(new { message = "No file uploaded." });
            }

            if (request.ProductId < 1)
            {
                return BadRequest(new { message = $"Not Accepted product id: {request.ProductId}" });
            }

            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(request.File.FileName);
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await request.File.CopyToAsync(fileStream);
            }

            var imageUrl = $"{Request.Scheme}://{Request.Host}/images/{uniqueFileName}";

            var dto = new ImageCreateDto
            {
                ImageUrl = imageUrl,
                ProductId = request.ProductId
            };

            var imageId = await _imageService.CreateImageAsync(dto);

            return CreatedAtAction(nameof(GetImageById), new { id = imageId }, new { id = imageId, imageUrl, message = "Image uploaded successfully." });
        }
    }
}
