using Infrastructure.Context;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Services;

internal class DatabaseInitializer(IServiceProvider serviceProvider, ILogger<DatabaseInitializer> logger)
    : IHostedService
{
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("Database initialization started...");

        using (var scope = serviceProvider.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<PortfolioDbContext>();

            try
            {
                logger.LogInformation("Checking database connectivity...");
                if (await dbContext.Database.CanConnectAsync(cancellationToken))
                    logger.LogInformation("Database is available.");
                else
                    logger.LogWarning("Database is unreachable!");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error checking database connectivity.");
            }
        }

        logger.LogInformation("Database initialization completed.");
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}