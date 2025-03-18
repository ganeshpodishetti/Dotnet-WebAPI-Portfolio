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
        services.AddScoped<IAuthenticationService, AuthenticationService>();
        services.AddScoped<IEducationService, EducationService>();
        services.AddScoped<IExperienceService, ExperienceService>();
        services.AddScoped<IProjectService, ProjectService>();
        services.AddScoped<ISkillService, SkillService>();
        services.AddScoped<IMessageService, MessageService>();
        services.AddScoped<ISocialLinkService, SocialLinkService>();

        return services;
    }
}