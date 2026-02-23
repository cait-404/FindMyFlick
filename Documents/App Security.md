Correct JWT configuration in Program.cs to update from defaults:
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
  .AddJwtBearer(options =>
  {
      var key = Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Secret"]!);
      options.TokenValidationParameters = new TokenValidationParameters
      {
          ValidateIssuerSigningKey = true,
          IssuerSigningKey = new SymmetricSecurityKey(key),
          ValidateIssuer = true,
          ValidIssuer = builder.Configuration["Jwt:Issuer"],
          ValidateAudience = true,
          ValidAudience = builder.Configuration["Jwt:Audience"],
          ValidateLifetime = true,
          ClockSkew = TimeSpan.FromSeconds(30),
      };
  });
  
  In services:
  private string GenerateJwtToken(ApplicationUser user)
{
    var claims = new List<Claim>
    {
        new Claim(JwtRegisteredClaimNames.Sub, user.Id),
        new Claim(JwtRegisteredClaimNames.Email, user.Email!),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        new Claim(JwtRegisteredClaimNames.Iat,
            DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(),
            ClaimValueTypes.Integer64)
    };

    var key = new SymmetricSecurityKey(
        Encoding.UTF8.GetBytes(_config["Jwt:Secret"]!));
    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

    var token = new JwtSecurityToken(
        issuer: _config["Jwt:Issuer"],
        audience: _config["Jwt:Audience"],
        claims: claims,
        expires: DateTime.UtcNow.AddMinutes(15),
        signingCredentials: creds);

    return new JwtSecurityTokenHandler().WriteToken(token);
}
 
generate JWT secret with "openssl rand -base64 64"

Reference: https://cheatsheetseries.owasp.org/cheatsheets/JSON_Web_Token_for_Java_Cheat_Sheet.html

Frontend JWT handling - never store JWTS in localStorage - use in-memory state for access tokens:
import { useState, useContext, createContext } from 'react';

const AuthContext = createContext(null);

export function AuthProvider({ children }) {
  const [accessToken, setAccessToken] = useState(null);

  const login = async (email, password) => {
    const res = await fetch('/api/auth/login', {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({ email, password }),
      credentials: 'include'
    });
    const data = await res.json();
    setAccessToken(data.accessToken);
  };

  return (
    <AuthContext.Provider value={{ accessToken, login }}>
      {children}
    </AuthContext.Provider>
  );
}

On the backend - using httpOnly cookies for refresh tokens - I think in AccountController.cs:
var cookieOptions = new CookieOptions
{
    HttpOnly = true,
    Secure = true,
    SameSite = SameSiteMode.Strict,
    Expires = DateTimeOffset.UtcNow.AddDays(7),
    Path = "/api/auth/refresh"
};
Response.Cookies.Append("refreshToken", refreshToken, cookieOptions);

return Ok(new { accessToken });

Postgresql - storing refresh hash in database:
var tokenHash = Convert.ToHexString(
    SHA256.HashData(Encoding.UTF8.GetBytes(refreshToken)));

var entry = new RefreshToken
{
    TokenHash = tokenHash,
    UserId = user.Id,
    ExpiresAt = DateTime.UtcNow.AddDays(7),
    CreatedAt = DateTime.UtcNow
};
_context.RefreshTokens.Add(entry);
await _context.SaveChangesAsync();

for reference: https://cheatsheetseries.owasp.org/cheatsheets/Session_Management_Cheat_Sheet.html
			   https://cheatsheetseries.owasp.org/cheatsheets/Cross_Site_Scripting_Prevention_Cheat_Sheet.html
			   
Proper logging configuration - in Program.cs:
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/error");
}

// In Error controller
[ApiController]
public class ErrorController : ControllerBase
{
    [Route("/error")]
    [AllowAnonymous]
    public IActionResult HandleError()
    {
        return Problem(
            title: "An unexpected error occurred.",
            statusCode: 500);
    }
}

Logging should be something like (below) - Serilog could be good for logging sinks, otherwise log to Postgresql.
_logger.LogWarning(
    "Failed login attempt for user {UserId} from IP {IpAddress}",
    userId,                 // Safe — internal identifier
    context.Connection.RemoteIpAddress);

_logger.LogWarning(
    "Account locked out: {UserId} after {Attempts} failed attempts",
    userId, maxAttempts);
	
To implement/consider dotnet user-secrets, for example:
dotnet user-secrets init
dotnet user-secrets set "Jwt:Secret" "your-super-secret-key-here-minimum-32-chars"
dotnet user-secrets set "Jwt:Issuer" "https://localhost:5001"
dotnet user-secrets set "Jwt:Audience" "https://localhost:3000"
dotnet user-secrets set "ExternalApis:TmdbApiKey" "your_tmdb_key"
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Host=...;Database=..."

Reference: https://learn.microsoft.com/en-us/aspnet/core/security/app-secrets?view=aspnetcore-10.0&tabs=windows
Also, Github secrets: https://docs.github.com/en/actions/how-tos/write-workflows/choose-what-workflows-do/use-secrets

To review:
Proper API Key security implementation
HTTPS is set update
Improper react set up - could lead to XSS issues