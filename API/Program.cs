using API.Extensions;
using Application.Extensions;
using Infrastructure.Extension;
using Scalar.AspNetCore;

try
{
    var builder = WebApplication.CreateBuilder(args);

    // Add services to the container.
    builder.AddPresentation();
    builder.Services.AddScalarOpenApi();
    builder.Services.AddDatabase();
    builder.Services.AddJwtAuthentication(builder.Configuration);
    builder.Services.AddInfrastructure();
    builder.Services.AddApplication();

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseDeveloperExceptionPage();
        app.MapOpenApi();
        app.MapScalarApiReference(opt =>
            opt
                .WithTitle("Portfolio API")
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
    app.UseAuthentication();
    app.UseAuthorization();
    app.MapControllers();

    app.Run();
}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
    throw new Exception("An error occurred while start-up the application.");
}