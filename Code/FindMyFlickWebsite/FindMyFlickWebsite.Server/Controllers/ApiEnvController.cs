using Microsoft.AspNetCore.Mvc;

namespace FindMyFlickWebsite.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ApiEnvController : ControllerBase
    {
        // GET /api/ApiEnv
        [HttpGet]
        public IActionResult Get()
        {
            var tmdb = Environment.GetEnvironmentVariable("TMDB_API_KEY") ?? "";
            var dtdd = Environment.GetEnvironmentVariable("DTDD_API_KEY") ?? "";

            return Ok(new
            {
                TMDB = string.IsNullOrWhiteSpace(tmdb) ? "" : "(set)",
                DTDD = string.IsNullOrWhiteSpace(dtdd) ? "" : "(set)"
            });
        }
    }
}