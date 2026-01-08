using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Swagger.Models
{
    public class ImageUploadDto
    {
        public IFormFile File { get; set; }
    }
    public class Images
    {
        [Key]
        public Guid ImageId { get; set; }
        public string UserId { get; set; }
        [MaxLength(500)]
        public string Url  { get; set; }
        public DateTime DateTime { get; set; }
        [ForeignKey(nameof(UserId))]
        public Account account { get; set; }

    }
}
