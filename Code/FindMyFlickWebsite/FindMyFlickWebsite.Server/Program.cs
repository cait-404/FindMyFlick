using FindMyFlickWebsite.Server;
using FindMyFlickWebsite.Server.DataModels;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.IdentityModel.Tokens.Jwt;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        policy =>
        {
            policy
                .WithOrigins("http://localhost:5173")
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});

builder.Services.AddControllers(options =>
    options.Filters.Add(new Microsoft.AspNetCore.Mvc.IgnoreAntiforgeryTokenAttribute()));

// Configure Swagger with JWT / API token support
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "FindMyFlick API", Version = "v1" });

    // Define the BearerAuth scheme that's in use
    var bearerScheme = new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter 'Bearer {your JWT token}' (without quotes)."
    };

    c.AddSecurityDefinition("Bearer", bearerScheme);

    // Require the bearer token for all operations (UI will present Authorize button)
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        { bearerScheme, Array.Empty<string>() }
    });
});

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// keep ApplicationDbContext for Identity migrations / tooling
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString)
    //.UseSnakeCaseNamingConvention()
);

// ===== ADDED: DbContextFactory for FindmyflickContext =====
builder.Services.AddDbContextFactory<FindmyflickContext>(options =>
    options.UseNpgsql(connectionString)
);

// Existing registration for direct FindmyflickContext (you may keep or remove if fully migrating to factory)
builder.Services.AddDbContext<FindmyflickContext>(options =>
    options.UseNpgsql(connectionString)
);

builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = true;
    options.Password.RequiredLength = 15;
})
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;

    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,

        // Must match values used when creating the token
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        IssuerSigningKey = new SymmetricSecurityKey(
            System.Text.Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!)),

        // Make role and name claim resolution consistent with the claims you emit
        RoleClaimType = System.Security.Claims.ClaimTypes.Role,
        NameClaimType = System.Security.Claims.ClaimTypes.NameIdentifier
    };

    // Optional: helpful diagnostics during development
    options.Events = new Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerEvents
    {
        OnMessageReceived = ctx =>
        {
            // Log the raw Authorization header and the token string
            var logger = ctx.HttpContext.RequestServices.GetRequiredService<ILogger<Program>>();
            var authHeader = ctx.Request.Headers["Authorization"].FirstOrDefault();
            // Sanitize header value to prevent log forging (strip line breaks)
            string SanitizeForLogging(string value)
            {
                if (string.IsNullOrEmpty(value))
                    return value;

                // Remove CRLF and standalone CR/LF to avoid log forging via new lines
                return value
                    .Replace(Environment.NewLine, string.Empty)
                    .Replace("\r", string.Empty)
                    .Replace("\n", string.Empty);
            }

            var safeAuthHeader = authHeader is null ? "(missing)" : SanitizeForLogging(authHeader);
            logger.LogInformation("OnMessageReceived - Authorization header: {Header}", safeAuthHeader);
            var token = ctx.Request.Headers.ContainsKey("Authorization")
                        ? ctx.Request.Headers["Authorization"].ToString().Split(' ').LastOrDefault()
                        : null;
            logger.LogInformation("OnMessageReceived - extracted token length: {Len}", token?.Length ?? 0);
            return Task.CompletedTask;
        },
        OnAuthenticationFailed = ctx =>
        {
            var logger = ctx.HttpContext.RequestServices.GetRequiredService<ILogger<Program>>();
            logger.LogError(ctx.Exception, "JWT authentication failed: {Message}", ctx.Exception.Message);
            // include inner exception if present
            if (ctx.Exception.InnerException != null)
                logger.LogError(ctx.Exception.InnerException, "Inner exception: {Message}", ctx.Exception.InnerException.Message);
            return Task.CompletedTask;
        },
        OnTokenValidated = ctx =>
        {
            var logger = ctx.HttpContext.RequestServices.GetRequiredService<ILogger<Program>>();
            var roles = string.Join(",", ctx.Principal?.FindAll(System.Security.Claims.ClaimTypes.Role).Select(c => c.Value) ?? Enumerable.Empty<string>());
            logger.LogInformation("OnTokenValidated - sub={Sub} nameid={NameId} roles={Roles}",
                ctx.Principal?.FindFirst(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Sub)?.Value,
                ctx.Principal?.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value,
                roles);
            return Task.CompletedTask;
        }
    };
});

builder.Services.AddAuthorization(
    options =>
    {
        options.AddPolicy("AdminPolicy", policy => policy.RequireRole("Admin"));
        options.AddPolicy("UserPolicy", policy => policy.RequireRole("User"));
    }
    );

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "FindMyFlick API v1");
        // The Authorize button will appear automatically because of the security definition.
    });
}

<<<<<<< HEAD
var builder = WebApplication.CreateBuilder(args);
=======
//app.UseHttpsRedirection(); for prod
app.UseRouting(); 
app.UseCors("AllowFrontend");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
>>>>>>> 4db7736f7051ced3958f6de6f2843d007280641b

// Add controllers
builder.Services.AddControllers();

// Add CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
        policy.WithOrigins("http://localhost:5173") // your React app
              .AllowAnyHeader()
              .AllowAnyMethod());
});

// If you have authentication later, configure it properly here
// builder.Services.AddAuthentication("YourScheme").AddYourAuthMethod();
// builder.Services.AddAuthorization();

var app = builder.Build();

// Development URLs
if (app.Environment.IsDevelopment())
{
    app.Urls.Clear();
    app.Urls.Add("https://localhost:5002");
    app.Urls.Add("http://localhost:5003");
}

<<<<<<< HEAD
// Middleware
app.UseHttpsRedirection();
app.UseRouting();

// CORS must come after UseRouting and before MapControllers
app.UseCors("AllowFrontend");

// Uncomment these when you configure authentication
// app.UseAuthentication();
// app.UseAuthorization();

app.MapControllers();

=======
>>>>>>> 4db7736f7051ced3958f6de6f2843d007280641b
app.Run();