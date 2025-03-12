using Domain.Entities;
using Domain.Options;
using Infrastructure.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Infrastructure.Extension;

public static class DatabaseExtension
{
    public static IServiceCollection AddDatabase(this IServiceCollection services)
    {
        // Registering the PortfolioDbContext
        services.AddDbContext<PortfolioDbContext>((provider, options) =>
        {
            var connectionString =
                provider.GetRequiredService<IOptions<ConnStringOptions>>().Value.PostgresSqlConnection;

            if (string.IsNullOrEmpty(connectionString))
                throw new InvalidOperationException("Connection string is not configured.");
            options.UseNpgsql(connectionString);
        });

        // Registering the Identity Services
        services.AddIdentity<User, IdentityRole<Guid>>(options =>
            {
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireDigit = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireLowercase = true;
                options.Password.RequiredLength = 8;
            })
            .AddEntityFrameworkStores<PortfolioDbContext>()
            .AddApiEndpoints()
            .AddDefaultTokenProviders();

        return services;
    }
}