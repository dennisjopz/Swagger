using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swagger.Services;

namespace Swagger.Controllers
{
    /// <summary>
    /// Controller for managing Swagger-related account operations.
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class SwaggerController : Controller
    {
        private readonly AppDbContext _context;
        private readonly ILogger<SwaggerController> _logger;
        private readonly AccountService _accountservice;

        public SwaggerController(AppDbContext context, ILogger<SwaggerController> logger, AccountService accountservice)
        {
            _context = context;
            _logger = logger;
            _accountservice = accountservice;
        }

        #region Get

        [HttpGet("GetAllAccounts")]
        public ActionResult<IEnumerable<Account>> GetAccounts()
        {
            try
            {
                return _context.Accounts.ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching accounts - GetAccounts");
                return StatusCode(500, new { message = "An unexpected error occurred.", detail = ex.Message });
            }
        }

        [HttpGet("GetAccountsById/{id}")]
        public ActionResult<IEnumerable<Account>> GetAccountsById(int id)
        {
            try
            {
                var accounts = _context.Accounts.Where(a => a.Id == id).ToList();

                if (!accounts.Any())
                {
                    return NotFound();
                }

                return Ok(accounts);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching accounts - GetAccountsById");
                return StatusCode(500, new { message = "An unexpected error occurred.", detail = ex.Message });
            }
        }

        [HttpGet("GetAccountsByName/{username}")]
        public ActionResult<IEnumerable<Account>> GetAccountsByName(string userName)
        {
            try
            {
                var accounts = _context.Accounts
                            .Where(a => a.Username.Contains(userName))
                            .ToList();

                if (!accounts.Any())
                {
                    return NotFound();
                }

                return Ok(accounts);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching accounts - GetAccountsByName");
                return StatusCode(500, new { message = "An unexpected error occurred.", detail = ex.Message });
            }

        }

        [HttpGet("GetAccountsByEmail/{email}")]
        public ActionResult<IEnumerable<Account>> GetAccountsByEmail(string email)
        {
            try
            {
                var accounts = _context.Accounts
                           .Where(a => a.Email == email)
                           .ToList();

                if (!accounts.Any())
                {
                    return NotFound();
                }

                return Ok(accounts);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching accounts - GetAccountsByEmail");
                return StatusCode(500, new { message = "An unexpected error occurred.", detail = ex.Message });
            }
        }

        #endregion

        #region Insert

        [HttpPost("SPSCreateAdminUser")]
        public ActionResult SPSCreateAdminUser(Account account)
        {
            return NoContent();
        }

        [HttpPost("createadminuser")]
        public async Task<IActionResult> CreateAdminUser(CreateUserAccount user)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if(_accountservice.CheckEmailExists(user.Email))
                return Conflict(new { message = "Email already exists." });

            var hasher = new PasswordHasher<CreateUserAccount>();
            var account = new Account
            {
                Email = user.Email,
                UserId = _accountservice.GenerateUniqueUserId(),
                Username = user.Username,
                Password = hasher.HashPassword(user, user.Password),
                IsAdmin = true,
                Status = true,
                AccountCreated = DateTime.Now,
            };

            try
            {
                _context.Accounts.Add(account);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(CreateAdminUser), new { id = account.Id }, account);

            }
            catch (DbUpdateException ex)
            {
                return Conflict(new { message = "Unable to save account. Possibly duplicate data.", detail = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while Creating accounts - CreateAdminUser");
                return StatusCode(500, new { message = "An unexpected error occurred.", detail = ex.Message });
            }
        }

        [HttpPost("CreateUser")]
        public async Task<IActionResult> CreateUser(CreateUserAccount user)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (_accountservice.CheckEmailExists(user.Email))
                return Conflict(new { message = "Email already exists." });

            var hasher = new PasswordHasher<CreateUserAccount>();
            var account = new Account
            {
                UserId = _accountservice.GenerateUniqueUserId(),
                Email = user.Email,
                Password = hasher.HashPassword(user, user.Password),
                Username = user.Username,
                IsAdmin = false,
                Status = true,
            };

            try
            {
                _context.Accounts.Add(account);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(CreateUser), new { id = account.Id }, account);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while Creating accounts - CreateUser");
                return StatusCode(500, new { message = "An unexpected error occurred.", detail = ex.Message });
            }
        }

        #endregion

        #region Update

        [HttpPut("updatepassword")]
        public async Task<IActionResult> UpdatePassword(UpdatePassword password)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var user = await _context.Accounts.FirstOrDefaultAsync(x => x.Username == password.Username);
                if (user == null)
                    return NotFound("User not found");

                user.Password = password.Password;
                await _context.SaveChangesAsync();
                return Ok(new { message = "Password updated successfully" });

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while Updating - UpdatePassword");
                return StatusCode(500, new { message = "An unexpected error occurred.", detail = ex.Message });
            }

        }

        #endregion

    }
}
