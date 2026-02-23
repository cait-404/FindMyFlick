using FindMyFlickWebsite.Server;
using FindMyFlickWebsite.Server.DataModels;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// ========================
// CORS CONFIGURATION
// ========================
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        policy =>
        {
            policy
                .WithOrigins(
                    "http://localhost:5173",  // Vite default
                    "http://localhost:5174"   // Your current port
                )
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials();
        });
});

// ========================
// SERVICES
// ========================
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Teammate DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString)
        .UseSnakeCaseNamingConvention()
);

// Scaffolded DB-first context
builder.Services.AddDbContext<FindmyflickContext>(options =>
    options.UseNpgsql(connectionString, o => o.CommandTimeout(60))
        .UseSnakeCaseNamingConvention()
);

var app = builder.Build();

// ========================
// MIDDLEWARE PIPELINE
// ========================
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Enable HTTPS redirect (recommended)
app.UseHttpsRedirection();

app.UseRouting();

// IMPORTANT: CORS must be between UseRouting and MapControllers
app.UseCors("AllowFrontend");

app.UseAuthorization();

app.MapControllers();

// Explicit ports
app.Urls.Clear();
app.Urls.Add("https://localhost:5002");
app.Urls.Add("http://localhost:5003");

app.Run();