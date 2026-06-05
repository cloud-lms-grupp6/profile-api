using Azure.Identity;
using System.Text;
using Lms.Profile.Application.Interfaces;
using Lms.Profile.Infrastructure.Data;
using Lms.Profile.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;

// Program.cs startar och konfigurerar Profile API.
// Här registreras databas, service-lager, autentisering och API-dokumentation.
//
// AI användes som stöd för att förstå konfigurationen av JWT, dependency injection
// och Azure Key Vault. Inställningarna anpassades därefter manuellt för LMS-projektet.

var builder = WebApplication.CreateBuilder(args);

// Hämtar secrets och connection strings från Azure Key Vault.
builder.Configuration.AddAzureKeyVault(
    new Uri("https://lms-kv-grupp6.vault.azure.net/"),
    new DefaultAzureCredential());

// Aktiverar controllers så att API-endpoints kan användas.
builder.Services.AddControllers();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy
            .WithOrigins("http://localhost:3000")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

// Kopplar Profile API till SQL Server via Entity Framework Core.
builder.Services.AddDbContext<ProfileDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Registrerar ProfileService i dependency injection.
// Detta gör att controller-lagret kan använda IProfileService.
builder.Services.AddScoped<IProfileService, ProfileService>();

// Hämtar JWT-inställningar från configuration.
// Fallback-värdet används endast för lokal utveckling.
var jwtKey = builder.Configuration["Jwt:SigningKey"]
    ?? "super-secret-development-key-change-this";

var jwtIssuer = builder.Configuration["Jwt:Issuer"] ?? "auth-api";
var jwtAudience = builder.Configuration["Jwt:Audience"] ?? "lms";

// Konfigurerar JWT Bearer authentication.
// Profile API accepterar bara requests med giltig token.
builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtIssuer,
            ValidAudience = jwtAudience,
            IssuerSigningKey = new SymmetricSecurityKey(Convert.FromBase64String(jwtKey))
        };
    });

// Aktiverar authorization så att [Authorize] fungerar i controllers.
builder.Services.AddAuthorization();

// Lägger till API-dokumentation via OpenAPI och Scalar.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi();

var app = builder.Build();

// Mappar OpenAPI och Scalar så att API:t kan testas i webbläsaren.
app.MapOpenApi();
app.MapScalarApiReference();

app.UseHttpsRedirection();

app.UseCors("AllowFrontend");

// Authentication måste köras före Authorization.
app.UseAuthentication();
app.UseAuthorization();

// Mappar alla controllers, till exempel ProfilesController.
app.MapControllers();

app.Run();