using FindMyFlickWebsite.Server;
using FindMyFlickWebsite.Server.DataModels;
using Microsoft.EntityFrameworkCore;
using System;

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

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// This is your teammate's app DbContext (keep it as-is)
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString)
        .UseSnakeCaseNamingConvention()
);

// This is the scaffolded DB-first context your controllers are using
builder.Services.AddDbContext<FindmyflickContext>(options =>
    options.UseNpgsql(connectionString, o => o.CommandTimeout(60))
        .UseSnakeCaseNamingConvention()
);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection(); for prod
app.UseRouting();
app.UseCors("AllowFrontend");
app.UseAuthorization();

app.MapControllers();
app.Urls.Clear();
app.Urls.Add("https://localhost:5002");
app.Urls.Add("http://localhost:5003");
app.Run();