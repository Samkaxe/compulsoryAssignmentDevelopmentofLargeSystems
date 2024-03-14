using System.Diagnostics;
using EasyNetQ;
using HistoryService.Application.Interfaces;
using HistoryService.Domain;
using Serilog;
using SharedConfiguration;


namespace HistoryService.API.Subscribers;

public class HistoryConsumer: BackgroundService
{
    
    private readonly IBus _bus;
    private readonly IServiceProvider _serviceProvider;
    private static readonly ActivitySource ActivitySource = new("HistoryService.HistoryConsumer");

    public HistoryConsumer(IBus bus, IServiceProvider serviceProvider)
    {
        _bus = bus;
        _serviceProvider = serviceProvider;
    }
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        stoppingToken.ThrowIfCancellationRequested();

        await _bus.PubSub.SubscribeAsync<OperationEntryMessage>("Operation_Entries", HandleOperationEntry, stoppingToken);
    }
    
    
    private async Task HandleOperationEntry(OperationEntryMessage dto)
    {
        using var activity = ActivitySource.StartActivity("HandleOperationEntry", ActivityKind.Server);
        
        if (activity == null)
        {
            Log.Warning("Activity was not created.");
            return;
        }
        
        activity?.SetTag("operation.type", dto.OperationType);
        activity?.SetTag("operation.operand1", dto.Operand1);
        activity?.SetTag("operation.operand2", dto.Operand2);
        activity?.SetTag("operation.result", dto.Result);
            
        try
        {  
            using var scope = _serviceProvider.CreateScope();
            var historyService = scope.ServiceProvider.GetRequiredService<IHistoryService>();

            var mappedEntry = new OperationEntry()
            {
                OperationType = dto.OperationType,
                Operand1 = dto.Operand1,
                Operand2 = dto.Operand2,
                Result = dto.Result,
                TimeStamp = dto.TimeStamp
            };
            await historyService.AddEntry(mappedEntry);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error processing message: {ex.Message}");
            activity?.SetStatus(ActivityStatusCode.Error, ex.Message);
        }
    }

}