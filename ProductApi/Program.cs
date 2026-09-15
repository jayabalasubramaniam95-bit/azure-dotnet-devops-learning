using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using System.Text;
using ProductApi.Services;
using ProductApi.Security;

var builder = WebApplication.CreateBuilder(args);

// =====================================================
// 1. CONFIGURATION
// =====================================================

var configuration = builder.Configuration;

// =====================================================
// 2. JWT AUTHENTICATION
// =====================================================

var jwtKey = builder.Configuration["Jwt:Key"]
    ?? throw new InvalidOperationException("JWT key is missing.");


var apiKey = builder.Configuration["ApiKey:Value"];


builder.Services.AddCors(options =>
{
    options.AddPolicy("FrontendPolicy", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme =
            JwtBearerDefaults.AuthenticationScheme;

        options.DefaultChallengeScheme =
            JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters =
            new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,

                ValidIssuer =
                    builder.Configuration["Jwt:Issuer"],

                ValidAudience =
                    builder.Configuration["Jwt:Audience"],

                IssuerSigningKey =
                    new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(jwtKey))
            };
    })
    .AddScheme<AuthenticationSchemeOptions,
        ApiKeyAuthenticationHandler>(
            "ApiKey",
            options =>
            {
            });

// =====================================================
// 3. AUTHORIZATION
// =====================================================

builder.Services.AddAuthorization();

// =====================================================
// 4. APPLICATION SERVICES
// =====================================================

builder.Services.AddScoped<JwtTokenService>();

// =====================================================
// 5. API SERVICES
// =====================================================

// Controllers
builder.Services.AddControllers();

// OpenAPI
builder.Services.AddOpenApi();

// Swagger
builder.Services.AddSwaggerGen(options =>
{
    // ==========================================
    // JWT / Bearer
    // ==========================================

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter your JWT token."
    });

    // ==========================================
    // API Key
    // ==========================================

    options.AddSecurityDefinition("ApiKey", new OpenApiSecurityScheme
    {
        Name = "X-API-Key",
        Type = SecuritySchemeType.ApiKey,
        In = ParameterLocation.Header,
        Description = "Enter your API key."
    });

    // ==========================================
    // Security requirements
    // ==========================================

    options.AddSecurityRequirement(document =>
        new OpenApiSecurityRequirement
        {
            [new OpenApiSecuritySchemeReference("Bearer", document)] = [],
            [new OpenApiSecuritySchemeReference("ApiKey", document)] = []
        });
});
// =====================================================
// 6. BUILD APPLICATION
// =====================================================

var app = builder.Build();

app.Use(async (context, next) =>
{
    if (context.Request.Path.StartsWithSegments("/api/products"))
    {
        if (!context.Request.Headers.TryGetValue("X-API-Key", out var providedApiKey))
        {
            context.Response.StatusCode = 401;
            await context.Response.WriteAsync("API key is missing.");
            return;
        }

        if (providedApiKey != apiKey)
        {
            context.Response.StatusCode = 401;
            await context.Response.WriteAsync("Invalid API key.");
            return;
        }
    }

    await next();
});

// =====================================================
// 7. HTTP REQUEST PIPELINE
// =====================================================

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

// =====================================================
// 8. CONTROLLERS
// =====================================================

app.MapControllers();

// =====================================================
// 9. PRODUCT ENDPOINT
// =====================================================
app.MapGet("/api-key-status", () =>
{
    if (string.IsNullOrEmpty(apiKey))
    {
        return Results.Problem("API key is not configured.");
    }

    return Results.Ok("API key is configured.");
});


app.MapGet("/api/products/{id}", (int id) =>
{
    var products = new[]
    {
        new { Id = 1, Name = "Laptop", Price = 1200 },
        new { Id = 2, Name = "Monitor", Price = 450 },
        new { Id = 3, Name = "Keyboard", Price = 100 }
    };

    var product = products.FirstOrDefault(p => p.Id == id);

    return product is not null
        ? Results.Ok(product)
        : Results.NotFound();
});

// =====================================================
// 10. HEALTH ENDPOINT
// =====================================================

app.MapGet("/health", () =>
{
    return Results.Ok(new
    {
        status = "Healthy",
        version = "1.0.0"
    });
});

// =====================================================
// 11. CONFIGURATION ENDPOINT
// =====================================================

app.MapGet("/config", () =>
{
    var apiVersion = configuration["ApiVersion"];

    var environmentValue =
        Environment.GetEnvironmentVariable("ApiVersion");

    return Results.Ok(new
    {
        apiVersion,
        environmentValue
    });
});


// =====================================================
// 12. RUN APPLICATION
// =====================================================

app.Run();

// Required for integration tests
public partial class Program
{
}

// Existing WeatherForecast model
record WeatherForecast(
    DateOnly Date,
    int TemperatureC,
    string? Summary)
{
    public int TemperatureF =>
        32 + (int)(TemperatureC / 0.5556);
}