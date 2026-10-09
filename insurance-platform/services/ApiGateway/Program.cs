using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplicationInsightsTelemetry();

// ============================================================
// JWT Authentication
// ============================================================

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

            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],

            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(
                    builder.Configuration["Jwt:Key"]!
                )
            )
        };
    });

// ============================================================
// Authorization
// ============================================================

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy(
        "RequireAuthenticatedUser",
        policy =>
        {
            policy.RequireAuthenticatedUser();
        });

    options.AddPolicy(
        "AgentOnly",
        policy =>
        {
            policy.RequireRole("Agent", "Admin");
        });

    options.AddPolicy(
        "AdminOnly",
        policy =>
        {
            policy.RequireRole("Admin");
        });
});

// ============================================================
// RATE LIMITING
// ============================================================
builder.Services.AddRateLimiter(options =>
{
    options.RejectionStatusCode =
        StatusCodes.Status429TooManyRequests;

    // ============================================
    // RATE LIMITING
    // ============================================

    options.AddPolicy("GlobalPolicy", context =>
    {
        return RateLimitPartition.GetFixedWindowLimiter(
            partitionKey:
                context.Connection.RemoteIpAddress?.ToString()
                ?? "unknown",

            factory: _ => new FixedWindowRateLimiterOptions
            {
                PermitLimit = 100,
                Window = TimeSpan.FromMinutes(1),
                QueueLimit = 0,
                AutoReplenishment = true
            });
    });

    // ============================================
    // THROTTLING
    // ============================================

    options.AddPolicy("QuoteThrottle", context =>
    {
        return RateLimitPartition.GetConcurrencyLimiter(
            partitionKey: "quote-service",

            factory: _ => new ConcurrencyLimiterOptions
            {
                PermitLimit = 10,

                QueueLimit = 20,

                QueueProcessingOrder =
                    QueueProcessingOrder.OldestFirst
            });
    });
});

// ============================================================
// YARP
// ============================================================

builder.Services
    .AddReverseProxy()
    .LoadFromConfig(
        builder.Configuration.GetSection("ReverseProxy")
    );

// ============================================================
// CORS
// ============================================================

builder.Services.AddCors(options =>
{
    options.AddPolicy("ReactPolicy", policy =>
    {
        policy
            .WithOrigins(
                "http://localhost:5173",
                "https://insurance-portal.livelyisland-a6b04717.eastus.azurecontainerapps.io"
            )
            .WithMethods(
                "GET",
                "POST",
                "PUT",
                "DELETE",
                "OPTIONS"
            )
            .WithHeaders(
                "Content-Type",
                "Authorization"
            );
    });
});

var app = builder.Build();

// ============================================================
// Middleware Pipeline
// ============================================================

app.UseCors("ReactPolicy");

app.UseAuthentication();
app.UseAuthorization();

app.UseRateLimiter();

// ============================================================
// JWT Token Endpoint
// ============================================================

app.MapPost("/auth/token", (TokenRequest request) =>
{
    if (request.Password != "Password123!")
    {
        return Results.Unauthorized();
    }

    var role = request.Username.ToLowerInvariant() switch
    {
        "admin" => "Admin",
        "agent" => "Agent",
        "customer" => "Customer",
        _ => null
    };

    if (role == null)
    {
        return Results.Unauthorized();
    }

    var claims = new[]
    {
        new Claim(
            ClaimTypes.Name,
            request.Username),

        new Claim(
            ClaimTypes.Role,
            role)
    };

    var key = new SymmetricSecurityKey(
        Encoding.UTF8.GetBytes(
            builder.Configuration["Jwt:Key"]!
        )
    );

    var credentials = new SigningCredentials(
        key,
        SecurityAlgorithms.HmacSha256);

    var token = new JwtSecurityToken(
        issuer: builder.Configuration["Jwt:Issuer"],
        audience: builder.Configuration["Jwt:Audience"],
        claims: claims,
        expires: DateTime.UtcNow.AddHours(1),
        signingCredentials: credentials);

    var tokenValue =
        new JwtSecurityTokenHandler()
            .WriteToken(token);

    return Results.Ok(new
    {
        accessToken = tokenValue,
        tokenType = "Bearer",
        role = role,
        expiresIn = 3600
    });
})
.RequireRateLimiting("GlobalPolicy");

// ============================================================
// YARP
// ============================================================

app.MapReverseProxy()
   .RequireRateLimiting("GlobalPolicy");

app.Run();

public record TokenRequest(
    string Username,
    string Password);