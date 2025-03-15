using Domain.Interfaces;
using Domain.UnitOfWork;
using Infrastructure.Repositories;
using Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Extension;

public static class InfrastructureExtension
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        // Registering the UnitOfWork
        services.AddTransient<IUnitOfWork, UnitOfWork.UnitOfWork>();

        // Registering the Repositories
        services.AddTransient<IUserRepository, UserRepository>();
        services.AddTransient<IIdentityRepository, IdentityRepository>();
        services.AddTransient<IEducationRepository, EducationRepository>();

        services.AddScoped<IJwtTokenService, JwtTokenService>();

        return services;
    }
}