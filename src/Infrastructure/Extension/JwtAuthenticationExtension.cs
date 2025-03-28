using System.Text;
using System.Text.Json;
using Domain.Entities;
using Domain.Options;
using Infrastructure.Context;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Extension;

public static class JwtAuthenticationExtension
{
    // Config JWT Authentication
    public static IServiceCollection AddJwtAuthentication(this IServiceCollection services,
        IConfiguration configuration)
    {
        //Checking JWT key
        var jwtSettings = configuration.GetSection("JwtConfig").Get<JwtTokenOptions>();
        if (jwtSettings == null || string.IsNullOrEmpty(jwtSettings.Key))
            throw new InvalidOperationException("JWT secret key is not configured.");

        var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key));

        // Registering the Authentication Services
        services.AddIdentity<User, IdentityRole<Guid>>()
            .AddEntityFrameworkStores<PortfolioDbContext>()
            .AddApiEndpoints()
            .AddDefaultTokenProviders();

        // JWT Authentication
        services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer("Bearer", options =>
            {
                options.RequireHttpsMetadata = false;
                //options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings.Issuer,
                    ValidAudience = jwtSettings.Audience,
                    IssuerSigningKey = secretKey,
                    ClockSkew = TimeSpan.Zero
                };
                options.Events = new JwtBearerEvents
                {
                    OnChallenge = context =>
                    {
                        context.HandleResponse();
                        context.Response.StatusCode = 401;
                        context.Response.ContentType = "application/json";
                        var result = JsonSerializer.Serialize(new
                        {
                            message = "You are not authorized to access this resource. Please authenticate."
                        });
                        return context.Response.WriteAsync(result);
                    }
                };
            });
        services.AddAuthorizationBuilder()
            .AddPolicy("RequireAdminRole", policy => policy.RequireRole("Admin"))
            .SetDefaultPolicy(new AuthorizationPolicyBuilder()
                .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                .RequireAuthenticatedUser()
                .Build());

        return services;
    }
}