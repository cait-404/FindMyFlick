using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace FindMyFlickWebsite.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DebugController : ControllerBase
    {
        // No auth so you can see what Swagger actually sends
        [HttpGet("echo")]
        [AllowAnonymous]
        public IActionResult EchoAuthHeader()
        {
            var header = Request.Headers.ContainsKey("Authorization")
                ? Request.Headers["Authorization"].ToString()
                : null;

            return Ok(new
            {
                AuthorizationHeader = header,
                Tip = "If this is null or doesn't start with 'Bearer ', Swagger/client didn't send the header correctly."
            });
        }

        // Requires valid token — returns claims produced by JwtBearer
        [HttpGet("claims")]
        [Authorize]
        public IActionResult Claims()
        {
            var claims = User.Claims.Select(c => new { c.Type, c.Value }).ToList();
            return Ok(new { Authenticated = User.Identity?.IsAuthenticated, Claims = claims });
        }
    }
}