using Domain.Options;
using Infrastructure.Extension;

try
{
    var builder = WebApplication.CreateBuilder(args);

    // Add services to the container.
    // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
    builder.Services.AddOpenApi();

    // Registering the DbContext
    builder.Services.Configure<ConnStringOptions>(
        builder.Configuration.GetSection(ConnStringOptions.ConnectionStrings));
    
    builder.Services.AddDatabase();
    
    builder.Services.AddControllers();

    var app = builder.Build();
    
    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.MapOpenApi();
    }

    app.UseHttpsRedirection();
    
    app.UseRouting();
    
    app.MapControllers();

    app.Run();
}
catch (Exception)
{
    throw new Exception("An error occurred while start-up the application.");
}



