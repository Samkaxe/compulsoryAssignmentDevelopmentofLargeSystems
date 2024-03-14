using EasyNetQ;
using HistoryService.API.Subscribers;
using HistoryService.Application.Interfaces;
using HistoryService.Infrastructure;
using HistoryService.Infrastructure.Interfaces;
using HistoryService.Infrastructure.Services;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Serilog;

namespace HistoryService;

public static class ConfigurationService
{
    public static void ConfigureServices(IServiceCollection services)
    {
        services.AddScoped<IHistoryRepository, HistoryRepository>();
        services.AddScoped<IHistoryService, Application.Services.HistoryService>();
        services.AddDbContext<HistoryContext>();

        var connectionString = Environment.GetEnvironmentVariable("EASYNETQ_CONNECTION_STRING");
        services.AddSingleton<IBus>(RabbitHutch.CreateBus(connectionString));

        services.AddHostedService<HistoryConsumer>();

        services.AddOpenTelemetry()
            .ConfigureResource(builder => builder.AddService("HistoryService"))
            .WithTracing(builder =>
                {
                    builder
                        .AddZipkinExporter(options =>
                            options.Endpoint = new Uri("http://zipkin:9411/api/v2/spans"))
                        .AddSource("HistoryService.HistoryConsumer")
                        .SetSampler(new AlwaysOnSampler())
                        .AddAspNetCoreInstrumentation()
                        .AddConsoleExporter();
                }
            );
        
        // Log.Logger = new LoggerConfiguration()
        //     .WriteTo.Seq("http://localhost:5341")
        //     .CreateLogger();

    }
}