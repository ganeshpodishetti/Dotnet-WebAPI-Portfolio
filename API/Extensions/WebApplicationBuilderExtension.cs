using API.Handlers;
using API.Helpers;
using Domain.Options;

namespace API.Extensions;

public static class WebApplicationBuilderExtension
{
    public static void AddPresentation(this WebApplicationBuilder builder)
    {
        // Registering the exception handler
        builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
        builder.Services.AddProblemDetails();

        // Registering the DbContext
        builder.Services.Configure<ConnStringOptions>(
            builder.Configuration.GetSection(ConnStringOptions.ConnectionStrings));
        builder.Services.Configure<JwtTokenOptions>(
            builder.Configuration.GetSection(JwtTokenOptions.JwtConfig));

        // Registering the services
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddHttpContextAccessor();

        // Registering the Services
        builder.Services.AddScoped<IAccessTokenHelper, AccessTokenHelper>();
    }
}