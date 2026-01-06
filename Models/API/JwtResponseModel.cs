namespace Swagger.Models.API
{
    public class JwtResponseModel
    {
        //public string? UserName { get; set; }
        public string? AccessToken { get; set; }
        public int ExpiresIn { get; set; }
    }
}
