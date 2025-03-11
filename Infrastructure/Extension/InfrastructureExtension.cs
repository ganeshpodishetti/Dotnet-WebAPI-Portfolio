using Domain.Entities;
using Domain.Interfaces;
using Domain.UnitOfWork;
using Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Extension;

public static class InfrastructureExtension
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        // Registering the UnitOfWork
        services.AddScoped<IUnitOfWork, UnitOfWork.UnitOfWork>();

        // Registering the Repositories
        services.AddScoped<IRepository<User>, Repository<User>>();
        services.AddScoped<IRepository<Skill>, Repository<Skill>>();
        services.AddScoped<IRepository<Project>, Repository<Project>>();
        services.AddScoped<IRepository<Message>, Repository<Message>>();
        services.AddScoped<IRepository<Education>, Repository<Education>>();
        services.AddScoped<IRepository<Experience>, Repository<Experience>>();
        services.AddScoped<IRepository<SocialLink>, Repository<SocialLink>>();

        return services;
    }
}