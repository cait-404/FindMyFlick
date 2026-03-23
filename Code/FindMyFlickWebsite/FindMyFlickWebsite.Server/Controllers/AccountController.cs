using FindMyFlickWebsite.Server.DataModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt; 
using System.Security.Claims;
using System.Linq;

//all of the code here is based on this tutorial https://www.youtube.com/watch?v=brxStRVyJiM . this also includes the code for the admin and user controllers, plus their datamodels and the code for the jwt authentication in the program.cs file.
namespace FindMyFlickWebsite.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        private IConfiguration _configuration;

        public AccountController(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;

        }
        //register a new user

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] Register model)
        {
            var userExists = await _userManager.FindByNameAsync(model.Username);
            if (userExists != null)
                return Conflict("User already exists!");
            IdentityUser user = new()
            {
                Email = model.Email,
                UserName = model.Username
            };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
                return Ok("User created successfully!");
            return BadRequest(result.Errors);
        }

        //login a user
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] Login model)
        {
            var user = await _userManager.FindByNameAsync(model.Username);
            if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
            {
                // Get roles from Identity
                var userRoles = await _userManager.GetRolesAsync(user);

                // Build base claims (include NameIdentifier so server can read user id)
                var authClaims = new List<Claim>
                {
                    new Claim(JwtRegisteredClaimNames.Sub, user.UserName ?? string.Empty),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(ClaimTypes.NameIdentifier, user.Id), // ensure user id present
                    new Claim(JwtRegisteredClaimNames.Email, user.Email ?? string.Empty)
                };

                // Add role claims in two forms to maximize compatibility:
                //  - ClaimTypes.Role (used by many ASP.NET APIs)
                //  - "role" (commonly used in JWT payloads and by some clients)
                foreach (var role in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, role));
                    authClaims.Add(new Claim("role", role));
                }

                var token = new JwtSecurityToken(
                    issuer: _configuration["Jwt:Issuer"],
                    expires: DateTime.Now.AddMinutes(double.Parse(_configuration["Jwt:ExpiryMinutes"]!)),
                    claims: authClaims,
                    signingCredentials: new Microsoft.IdentityModel.Tokens.SigningCredentials(
                        new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(
                            System.Text.Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!)),
                        Microsoft.IdentityModel.Tokens.SecurityAlgorithms.HmacSha256)
                    );

                return Ok(new { Token = new JwtSecurityTokenHandler().WriteToken(token) });
            }
            return Unauthorized("Invalid username or password!");
        }

        //add role
        [HttpPost("add-role")]
        public async Task<IActionResult> AddRole([FromBody] string role)
        {
            if(!await _roleManager.RoleExistsAsync(role))
            {   var result = await _roleManager.CreateAsync(new IdentityRole(role));
                if (result.Succeeded)
                    return Ok("Role created successfully!");
                return BadRequest(result.Errors);
            }
            return Conflict("Role already exists!");

        }

        [HttpPost("assign-role")]
        public async Task<IActionResult> AssignRole([FromBody] UserRole model)
        {
            var user = await _userManager.FindByNameAsync(model.Username);
            if (user == null)
                return NotFound("User not found!");
            if (!await _roleManager.RoleExistsAsync(model.Role))
                return NotFound("Role not found!");
            var result = await _userManager.AddToRoleAsync(user, model.Role);
            if (result.Succeeded)
                return Ok("Role assigned to user successfully!");
            return BadRequest(result.Errors);
        }
    }
}
