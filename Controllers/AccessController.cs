using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swagger.Models.API;
using Swagger.Services;

namespace Swagger.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class AccessController : Controller
    {
        private readonly JwtService _jwtService;
        private readonly AppDbContext _context;
        private readonly ILogger<SwaggerController> _logger;

        public AccessController(AppDbContext context, ILogger<SwaggerController> logger, JwtService jwtService)
        {
            _context = context;
            _logger = logger;
            _jwtService = jwtService;
        }

        [AllowAnonymous]
        [HttpPost("GetToken")]
        public async Task<ActionResult> GetToken(LoginRequestModel loginRequest)
        {
            var result = await _jwtService.Authenticate(loginRequest);
            if (result == null)
            {
                return Unauthorized("Invalid Username or Password");
            }

            return Ok(result);
        }
    }
}
