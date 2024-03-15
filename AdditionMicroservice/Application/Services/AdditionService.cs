using AdditionMicroservice.Application.DTOs;
using AdditionMicroservice.Application.Interfaces;

namespace AdditionMicroservice.Application.Services;

public class AdditionService: IAdditionService
{

    private readonly IPublisher _publisher;

    public AdditionService(IPublisher publisher)
    {
        _publisher = publisher;
    }

    public decimal Add(AdditionOperationDto dto)
    {
        _publisher.PublishOperationEntryMessage(dto);
        return dto.Operand1 + dto.Operand2;
    }
}