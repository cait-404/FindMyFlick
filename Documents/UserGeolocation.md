Limiting user geolocation to avoid GDPR violations -- 
MaxMind GeoLite2 is an open source GeoIp database which helps locate internet visitors - this will be used for geolocation IP restriction in tandem with explicit user
residency declaration via checkbox upon registration.

Use the following step below, some code is already implemented, use highlights to help indicate required changes/updates. 
You must first sign up and download the .mmdb file from MaxMind to obtain the database.

dotnet add package MaxMind.GeoIPs //add .mmdb file to project root - this should be added to .gitignore due to size and frequency of updates needed

Create a "GeoLocationServices.cs" service.
using MaxMind.GeoIP2;
using MaxMind.GeoIP2.Exceptions;

namespace FindMyFlickWebsite.Services;

public class GeoLocationService : IDisposable
{
    private readonly DatabaseReader _reader;
    private readonly ILogger<GeoLocationService> _logger;

    public GeoLocationService(
        IWebHostEnvironment env,
        ILogger<GeoLocationService> logger)
    {
        _logger = logger;
        var dbPath = Path.Combine(env.ContentRootPath, "GeoLite2-Country.mmdb");

        if (!File.Exists(dbPath))
            throw new FileNotFoundException(
                "GeoLite2 database not found. Download it from maxmind.com.", dbPath);

        _reader = new DatabaseReader(dbPath);
    } 
	
	 public string GetCountryCode(string? ipAddress)
    {
        if (string.IsNullOrWhiteSpace(ipAddress))
            return "UNKNOWN";

        // Localhost addresses during development
        if (ipAddress is "::1" or "127.0.0.1")
            return "LOCALHOST";

        try
        {
            var response = _reader.Country(ipAddress);
            return response.Country.IsoCode ?? "UNKNOWN";
        }
        catch (AddressNotFoundException)
        {
            // IP not in the database — treat as non-US (fail closed)
            _logger.LogWarning("GeoIP lookup: address not found in database: {Ip}", ipAddress);
            return "UNKNOWN";
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "GeoIP lookup failed for IP: {Ip}", ipAddress);
            return "UNKNOWN";
        }
    }

    public bool IsUnitedStates(string? ipAddress) =>
        GetCountryCode(ipAddress) == "US";

    public void Dispose() => _reader?.Dispose();
}


This should be in a user model file, I could be wrong, but can't find current equivalent in repo.
using Microsoft.AspNetCore.Identity;

namespace FindMyFlickWebsite.Models;

public class ApplicationUser : IdentityUser
{
    // --- US Residency Declaration ---

    // Did the user check the "I am a US resident" box?
    public bool UsResidencyConfirmed { get; set; }

    // UTC timestamp of when they checked it
    public DateTime? UsResidencyConfirmedAt { get; set; }

    // --- GeoIP Evidence ---

    // The IP address at time of registration
    public string? RegistrationIp { get; set; }

    // The country code MaxMind resolved that IP to (e.g. "US", "UNKNOWN")
    public string? DetectedCountry { get; set; }
}


I believe this would go into the "Register.cs" model, the reference used DTOs, so correct if wrong.
using System.ComponentModel.DataAnnotations;

namespace FindMyFlickWebsite.Models; //originally RegisterDto

public class Register
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = null!;

    [Required]
    [MinLength(12)]
    public string Password { get; set; } = null!;

    // Must be explicitly true or fails validation
    [MustBeTrue(ErrorMessage = "You must confirm US residency to register.")]
    public bool UsResidencyConfirmed { get; set; }
}

// Recommended ustom validation attribute — reusable across the board
public class MustBeTrueAttribute : ValidationAttribute
{
    public override bool IsValid(object? value) =>
        value is bool b && b;
}


This would go into the "AccountController.cs"
[AllowAnonymous]
[HttpPost("register")]
public async Task<IActionResult> Register(
    [FromBody] RegisterDto dto,
    [FromServices] GeoLocationService geoService)
{
    if (!ModelState.IsValid)
        return BadRequest(ModelState);

    // IP geolocation check
    var ip = HttpContext.Connection.RemoteIpAddress?.ToString();

    if (HttpContext.Request.Headers.TryGetValue("X-Forwarded-For", out var forwarded))
        ip = forwarded.ToString().Split(',')[0].Trim();

    var detectedCountry = geoService.GetCountryCode(ip);
    var isUsIp = detectedCountry == "US";

    // Checkbox self-declaration check - Requires both to pass
    if (!isUsIp || !dto.UsResidencyConfirmed)
    {
        return StatusCode(451, new
        {
            message = "This service is only available to US residents."
        });
    }

    // Create the user and store evidence
    var user = new ApplicationUser
    {
        UserName = dto.Email,
        Email = dto.Email,
        UsResidencyConfirmed = dto.UsResidencyConfirmed,
        UsResidencyConfirmedAt = DateTime.UtcNow,
        RegistrationIp = ip,
        DetectedCountry = detectedCountry,
    };

    var result = await _userManager.CreateAsync(user, dto.Password);

    if (!result.Succeeded)
        return BadRequest(result.Errors);

    return Ok(new { message = "Registration successful." });
}

In the "Program.cs" file, add
using Microsoft.AspNetCore.HttpOverrides;
using YourApp.Services;

var builder = WebApplication.CreateBuilder(args);

// existing Identity, EF Core, JWT setup ...

// Register GeoLocationService as singleton
builder.Services.AddSingleton<GeoLocationService>();

// This will be needed once code is hosted
builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders =
        ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;

    options.KnownNetworks.Clear();
    options.KnownProxies.Clear();
});

var app = builder.Build();

// Must be first in the middleware pipeline
app.UseForwardedHeaders();

Generate and apply the migration
dotnet ef migrations add AddGeoAndConsentFields
dotnet ef database update