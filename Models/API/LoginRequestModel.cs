using System.ComponentModel.DataAnnotations;

namespace Swagger.Models.API
{
    public class LoginRequestModel
    {
        [Required]
        public string? Email { get; set; }
        [Required]
        public string? password { get; set; }
    }
}
