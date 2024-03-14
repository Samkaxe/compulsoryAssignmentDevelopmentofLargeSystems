using EasyNetQ;
using HistoryService.API.Subscribers;
using HistoryService.Application.Interfaces;
using HistoryService.Infrastructure;
using HistoryService.Infrastructure.Interfaces;
using HistoryService.Infrastructure.Services;

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
    }
}