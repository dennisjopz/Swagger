using Microsoft.EntityFrameworkCore;
using Swagger.Controllers;

namespace Swagger.Services
{
    public class AccountService
    {
        private readonly AppDbContext _context;
        private readonly ILogger<SwaggerController> _logger;
        public AccountService(AppDbContext context, ILogger<SwaggerController> logger)
        {
            _context = context;
            _logger = logger;
        }

        public string GenerateUniqueUserId()
        {
            string userId = string.Empty;
            try
            {
                var guid = Guid.NewGuid();
                string shortId = Convert.ToBase64String(guid.ToByteArray())
                    .Replace("/", "_").Replace("+", "-").Substring(0, 22);

                userId = $"UID{shortId}";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred GenerateUniqueUserId");
            }

            return userId;
        }
        public bool CheckEmailExists(string email)
        {
            bool ret = false;
            try
            {
                return ret = _context.Accounts
                           .Any(a => a.Email == email);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error Occured CheckEmailExists");
                return ret;
            }
        }
    }

}
