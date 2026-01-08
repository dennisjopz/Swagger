namespace Swagger.Models.API
{
    public class JwtResponseModel
    {
        public string UserID { get; set; }
        public string? AccessToken { get; set; }
        public int ExpiresIn { get; set; }
    }
}
