using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swagger.Models;
using Swagger.Services;
using Swagger.Extensions;

namespace Swagger.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class ImageController : Controller
    {
        private readonly AppDbContext _context;
        private readonly ILogger<ImageController> _logger;
        private readonly ImageService _imageService;
        public ImageController(AppDbContext context, ILogger<ImageController> logger, ImageService imageService)
        {
            _context = context;
            _logger = logger;
            _imageService = imageService;
        }

        [HttpPost("uploadImage")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UploadImage([FromForm] ImageUploadDto image)
        {
            try
            {
                var file = image.File;
                if (!_imageService.ImageFormatValidation(file, out string formatError))
                    return BadRequest(formatError);

                //if (!_imageService.ImageSizeValidation(file, out string sizeError))
                //    return BadRequest(sizeError);

                var imageurl = await _imageService.StoreImage(file);

                var img = new Images
                {
                    UserId = User.GetUserId(),
                    Url = imageurl,
                    DateTime = DateTime.Now,
                };

                _context.Images.Add(img);
                await _context.SaveChangesAsync();
                return Ok(img);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while Creating accounts - CreateAdminUser");
                return StatusCode(500, new { message = "An unexpected error occurred.", detail = ex.Message });
            }
        }

    }
}
