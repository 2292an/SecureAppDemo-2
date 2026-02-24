using System.Text;
using System.Threading.RateLimiting;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SecureAppDemo.Data.Context;
using SecureAppDemo.Data.Extensions;
using SecureAppDemo.Repositories;
using SecureAppDemo.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .AddJsonFile("appsettings.local.json", optional: true, reloadOnChange: true);

// builder.Services.AddControllers();

builder.Services.AddDbContext<DatabaseContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var jwtSettings = builder.Configuration.GetSection("Jwt");

builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            ValidIssuer = jwtSettings["Issuer"],
            ValidAudience = jwtSettings["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtSettings["Secret"]))
        };
    });

builder.Services.AddRateLimiter(options =>
{
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

    options.AddFixedWindowLimiter("Fixed", limiterOptions =>
    {
        limiterOptions.PermitLimit = 3;
        limiterOptions.Window = TimeSpan.FromSeconds(20);
        limiterOptions.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
        limiterOptions.QueueLimit = 0;
    });
});

builder.Services.AddAuthorization();

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddControllers(); //registra ya como JSOn evita otros tipos de archivos como XML, etc. y se asegura que el API solo responda con JSON aunque el cliente pida otro tipo de contenido

builder.WebHost.ConfigureKestrel(options =>
{
    options.Limits.MaxRequestBodySize = 1024 * 1024; // 1MB
});

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IAuthorizationService, AuthorizationService>();
builder.Services.AddScoped<IAuthorizationRepository, AuthorizationRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || app.Environment.IsEnvironment("Demo"))
{
    app.MapOpenApi();

    var seedDataConfig = builder.Configuration.GetSection("SeedData");
    await app.Services.SeedDataAsync(seedDataConfig);
}

app.UseHttpsRedirection();
app.UseRateLimiter();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();