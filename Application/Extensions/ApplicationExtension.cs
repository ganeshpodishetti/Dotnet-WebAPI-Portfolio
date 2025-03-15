using Application.Interfaces;
using Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Extensions;

public static class ApplicationExtension
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        var applicationAssembly = typeof(ApplicationExtension).Assembly;

        // Auto Mapper
        services.AddAutoMapper(applicationAssembly);

        // Registering the UserServices
        services.AddScoped<IUserServices, UserServices>();
        services.AddScoped<IIdentityService, IdentityService>();
        services.AddScoped<IEducationService, EducationService>();

        return services;
    }
}