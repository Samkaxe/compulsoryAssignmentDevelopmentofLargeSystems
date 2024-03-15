using System.Diagnostics;
using AdditionMicroservice.Application.DTOs;
using AdditionMicroservice.Application.Interfaces;
using EasyNetQ;
using SharedConfiguration;

namespace AdditionMicroservice.Application.Services;

public class Publisher: IPublisher
{
    private readonly IBus _bus;

    public Publisher(IBus bus)
    {
        _bus = bus;
    }

    public async Task PublishOperationEntryMessage(AdditionOperationDto dto)
    {
        OperationEntryMessage message = MapToOperationEntryMessage(dto);

        var messageProperties = new MessageProperties();
        var currentActivity = Activity.Current;
        
        messageProperties.Headers?.Add("rootId",currentActivity?.RootId);

        await _bus.PubSub.PublishAsync<OperationEntryMessage>(message);
    }
    
    private OperationEntryMessage MapToOperationEntryMessage(AdditionOperationDto dto)
    {
        return new OperationEntryMessage
        {
            OperationType = "Addition",
            Operand1 = dto.Operand1,
            Operand2 = dto.Operand2,
            TimeStamp = dto.TimeStamp
        };
    }
}