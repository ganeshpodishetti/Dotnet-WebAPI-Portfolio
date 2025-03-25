using API.Extensions;
using Application.Extensions;
using Infrastructure.Extension;
using Scalar.AspNetCore;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    // .WriteTo.OpenTelemetry(options =>
    // {
    //     options.Endpoint = "http://localhost:5341/ingest/otlp/v1/traces";
    //     options.Protocol = OtlpProtocol.HttpProtobuf;
    // })
    .CreateLogger();

try
{
    Log.Information("Starting up the application...");

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

    app.UseSerilogRequestLogging();

    app.UseCors(policyBuilder => policyBuilder
        .AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader());

    app.UseStatusCodePages();
    app.UseExceptionHandler(_ => { });
    app.UseHttpsRedirection();
    app.UseRouting();
    app.UseAuthentication();
    app.UseAuthorization();
    app.MapControllers();
    app.MapHealthChecks("/health");

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "An error occurred while start-up the application.");
}
finally
{
    Log.CloseAndFlush();
}