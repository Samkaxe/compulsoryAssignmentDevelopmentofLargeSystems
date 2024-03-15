using AdditionMicroservice.Application.Interfaces;
using AdditionMicroservice.Application.Services;
using EasyNetQ;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace AdditionMicroservice;

public static class ConfigurationService
{
    public static void ConfigureServices(IServiceCollection services)
    {
        services.AddScoped<IAdditionService, AdditionService>();
        services.AddScoped<IPublisher, Publisher>();
        
        var connectionString = Environment.GetEnvironmentVariable("EASYNETQ_CONNECTION_STRING");
        services.AddSingleton<IBus>(RabbitHutch.CreateBus(connectionString));
        
        services.AddOpenTelemetry()
            .ConfigureResource(builder => builder.AddService("AdditionService"))
            .WithTracing(builder =>
                {
                    builder
                        .AddZipkinExporter(options =>
                            options.Endpoint = new Uri("http://zipkin:9411/api/v2/spans"))
                        .SetSampler(new AlwaysOnSampler())
                        .AddAspNetCoreInstrumentation()
                        .AddConsoleExporter();
                }
            );
    }
}