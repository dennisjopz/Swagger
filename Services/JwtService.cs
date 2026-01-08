using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Swagger.Controllers;
using Swagger.Models.API;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Swagger.Services
{
    public class JwtService
    {
        private readonly AppDbContext _dbContext;
        private readonly IConfiguration _configuration;
        private readonly ILogger<SwaggerController> _logger;
        public JwtService(AppDbContext dbContext, IConfiguration configuration, ILogger<SwaggerController> logger)
        {
            _dbContext = dbContext;
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<JwtResponseModel?> Authenticate(LoginRequestModel loginRequest)
        {
            try
            {
                var user = await _dbContext.Accounts
                                .SingleOrDefaultAsync(a => a.Email == loginRequest.Email);

                if (user == null)
                {
                    return null;
                }

                var hasher = new PasswordHasher<Account>();
                var result = hasher.VerifyHashedPassword(
                    user,
                    user.Password,
                    loginRequest.password
                );

                if (result == PasswordVerificationResult.Success)
                {
                    var issuer = _configuration["JwtConfig:Issuer"];
                    var audience = _configuration["JwtConfig:Audience"];
                    var key = _configuration["JwtConfig:key"];
                    var tokenValidityMin = _configuration.GetValue<int>("JwtConfig:ExpireMinutes");
                    var jwtSubject = _configuration["JwtConfig:Subject"];
                    var tokenExpiryTimeStamp = DateTime.UtcNow.AddMinutes(tokenValidityMin);

                    var claims = new[]
                    {
                        new Claim(JwtRegisteredClaimNames.Sub, user.UserId), // JWT standard
                        new Claim(ClaimTypes.NameIdentifier, user.UserId)    // maps correctly in ASP.NET Core
                    };

                    //var claims = new[]
                    //{
                    //    new Claim(JwtRegisteredClaimNames.Sub,jwtSubject),
                    //    new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
                    //    new Claim(ClaimTypes.NameIdentifier, user.UserId),
                    //};

                    var tokenDescriptor = new SecurityTokenDescriptor
                    {
                        Subject = new ClaimsIdentity(claims),
                        Expires = tokenExpiryTimeStamp,
                        Issuer = issuer,
                        Audience = audience,
                        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key)), SecurityAlgorithms.HmacSha512Signature),
                    };

                    var tokenHandler = new JwtSecurityTokenHandler();
                    var securityToken = tokenHandler.CreateToken(tokenDescriptor);
                    var accessToken = tokenHandler.WriteToken(securityToken);

                    return new JwtResponseModel
                    {
                        UserID = user.UserId,
                        AccessToken = accessToken,
                        ExpiresIn = (int)tokenExpiryTimeStamp.Subtract(DateTime.UtcNow).TotalSeconds,
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred JWTAuthenticate");
            }

            return null;
        }
        public bool ValidateLogin(Account user, string password)
        {
            try
            {
                var hasher = new PasswordHasher<Account>();
                var result = hasher.VerifyHashedPassword(user, user.Password, password);

                return result == PasswordVerificationResult.Success;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred ValidateLogin");
                return false;
            }
        }
    }
}
