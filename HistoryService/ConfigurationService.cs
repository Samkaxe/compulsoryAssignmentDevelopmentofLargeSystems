using HistoryService.Infrastructure;
using HistoryService.Infrastructure.Interfaces;
using HistoryService.Infrastructure.Services;

namespace HistoryService;

public static class ConfigurationService
{
    public static void ConfigureServices(IServiceCollection services)
    {
        services.AddScoped<IHistoryRepository, HistoryRepository>();
        services.AddDbContext<HistoryContext>();
    }
}