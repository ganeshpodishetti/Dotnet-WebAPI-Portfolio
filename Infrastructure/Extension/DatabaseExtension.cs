using Domain.Options;
using Infrastructure.Context;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Infrastructure.Extension;

public static class DatabaseExtension
{
    public static IServiceCollection AddDatabase(this IServiceCollection services)
    {
        // Registering the PortfolioDbContext
        services.AddDbContextPool<PortfolioDbContext>((provider, options) =>
        {
            var connectionString =
                provider.GetRequiredService<IOptions<ConnStringOptions>>().Value.PostgresSqlConnection;

            if (string.IsNullOrEmpty(connectionString))
                throw new InvalidOperationException("Connection string is not configured.");

            // Configure the DbContext
            options.UseNpgsql(connectionString, npgsqlOptions =>
            {
                npgsqlOptions.EnableRetryOnFailure(3);
                npgsqlOptions.CommandTimeout(30);
            });
        }, 32);

        // Registering the DatabaseInitializer
        services.AddHostedService<DatabaseInitializer>();

        return services;
    }
}