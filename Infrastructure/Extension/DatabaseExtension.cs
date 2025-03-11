using Domain.Options;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
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
            var connectionString = provider.GetRequiredService<IOptions<ConnStringOptions>>().Value.PostgresSqlConnection;
            
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new InvalidOperationException("Connection string is not configured.");
            }
            //var connectionString = configuration.GetConnectionString("PostgresSqlConnection");
            
            options.UseNpgsql(connectionString);
        });
        
        return services;
    }
}