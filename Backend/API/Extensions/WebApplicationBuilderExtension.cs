using System.Diagnostics;
using API.Handlers;
using API.Helpers;
using Domain.Options;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.ResponseCompression;
using OpenTelemetry;
using OpenTelemetry.Exporter;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Serilog;
using ExportProcessorType = OpenTelemetry.ExportProcessorType;

namespace API.Extensions;

public static class WebApplicationBuilderExtension
{
    public static void AddPresentation(this WebApplicationBuilder builder)
    {
        // Registering the Serilog logger
        builder.Host.UseSerilog((context, loggerConfiguration) =>
        {
            loggerConfiguration.ReadFrom.Configuration(context.Configuration);
        });

        // Registering the OpenTelemetry
        builder.Services
            .AddOpenTelemetry()
            .ConfigureResource(resource => resource.AddService(builder.Environment.ApplicationName))
            .WithTracing(tracing =>
            {
                tracing.AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation()
                    .AddEntityFrameworkCoreInstrumentation();

                tracing.AddOtlpExporter(opt =>
                {
                    opt.Endpoint = new Uri("http://localhost:5341/ingest/otlp/v1/traces");
                    opt.Protocol = OtlpExportProtocol.HttpProtobuf;
                    opt.ExportProcessorType = ExportProcessorType.Batch;
                    opt.BatchExportProcessorOptions = new BatchExportProcessorOptions<Activity>
                    {
                        MaxQueueSize = 1000,
                        ExporterTimeoutMilliseconds = 500, // Shorter timeout
                        MaxExportBatchSize = 100,
                        ScheduledDelayMilliseconds = 1000
                    };
                });
            })
            .WithMetrics(metrics =>
            {
                metrics.AddRuntimeInstrumentation()
                    .AddMeter(
                        "Microsoft.AspNetCore.Hosting",
                        "Microsoft.AspNetCore.Server.Kestrel",
                        "System.Net.Http",
                        "API",
                        "Microsoft.EntityFrameworkCore");
            });

        // Registering the CORS policy
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("myPolicyName",
                policy =>
                {
                    policy.AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
            options.AddPolicy("AllowOnlySomeOrigins",
                policy => { policy.WithOrigins("http://localhost:3000/"); });
        });

        // Registering the Response Compression
        builder.Services.AddResponseCompression(options =>
        {
            options.EnableForHttps = true;
            options.Providers.Add<GzipCompressionProvider>();
        });

        // Registering the exception handler
        builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
        builder.Services.AddProblemDetails(options =>
        {
            options.CustomizeProblemDetails = context =>
            {
                context.ProblemDetails.Instance =
                    $"{context.HttpContext.Request.Method} {context.HttpContext.Request.Path}";

                context.ProblemDetails.Extensions.TryAdd("requestId", context.HttpContext.TraceIdentifier);

                var activity = context.HttpContext.Features.Get<IHttpActivityFeature>()?.Activity;
                context.ProblemDetails.Extensions.TryAdd("traceId", activity?.Id);
            };
        });

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
        builder.Services.AddScoped<IFormatValidation, FormatValidation>();
    }
}