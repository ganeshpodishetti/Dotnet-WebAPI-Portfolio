using API.Extensions;
using Application.Extensions;
using Domain.Entities;
using Infrastructure.Extension;
using Scalar.AspNetCore;

try
{
    var builder = WebApplication.CreateBuilder(args);

    // Add services to the container.
    builder.AddPresentation();
    builder.Services.AddScalarOpenApi();
    builder.Services.AddDatabase();
    builder.Services.AddInfrastructure();
    builder.Services.AddApplication();

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.MapOpenApi();
        app.MapScalarApiReference(opt =>
            opt
                .WithTitle("Restaurant API")
                .WithTheme(ScalarTheme.Mars)
                .WithDarkMode(true)
                .WithSidebar(true)
                .WithDefaultHttpClient(ScalarTarget.Http, ScalarClient.Http11)
                .Authentication = new ScalarAuthenticationOptions
            {
                PreferredSecurityScheme = "Bearer"
            });
    }

    app.UseHttpsRedirection();

    app.UseRouting();

    app.MapGroup("api/identity")
        .WithTags("Identity")
        .MapIdentityApi<User>();

    app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllers();

    app.Run();
}
catch (Exception ex)
{
    throw new Exception("An error occurred while start-up the application.");
}