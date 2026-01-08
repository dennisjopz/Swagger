using Swagger.Controllers;

namespace Swagger.Services
{
    public class ImageService
    {
        private readonly AppDbContext _dbContext;
        private readonly ILogger<ImageService> _logger;
        private readonly IWebHostEnvironment _env;
        public ImageService(AppDbContext dbContext, ILogger<ImageService> logger, IWebHostEnvironment env)
        {
            _dbContext = dbContext;
            _logger = logger;
            _env = env ?? throw new ArgumentNullException(nameof(env));
        }
        public bool ImageFormatValidation(IFormFile images, out string error)
        {
            bool ret = false;
            error = string.Empty;
            try
            {
                if (images == null || images.Length == 0)
                {
                    error = "No Image Uploaded";
                    return false;
                }

                var allowedTypes = new[] { "image/jpeg", "image/jpg", "image/png", "image/webp" };

                if (!allowedTypes.Contains(images.ContentType))
                {
                    error = "Invalid image type";
                    return false;
                }

                return true;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred ImageValidation");
                error = ex.ToString();
            }
            return ret;
        }
        public bool ImageSizeValidation(IFormFile images, out string error)
        {
            error = string.Empty;
            bool ret = false;
            try
            {
                if (images == null || images.Length == 0)
                {
                    error = "No Image Uploaded";
                    return false;
                }

                if (images.Length > 5_000_000)
                {
                    error = "Image Size greater than 50MB";
                    return false;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred ImageValidation");
                error = ex.ToString();
            }

            return ret;
        }
        public async Task<string> StoreImage(IFormFile file)
        {
            try
            {

                var folderPath = Path.Combine(_env.WebRootPath, "uploads", "images");
               
                Directory.CreateDirectory(folderPath);

                var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
                var filePath = Path.Combine(folderPath, fileName);

                using var stream = new FileStream(filePath, FileMode.Create);
                await file.CopyToAsync(stream);

                return $"/uploads/images/{fileName}";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred StoreImage");
            }

            return string.Empty;
        }
    }
}