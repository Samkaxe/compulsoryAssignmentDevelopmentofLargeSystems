using AdditionMicroservice.Application.DTOs;

namespace AdditionMicroservice.Application.Interfaces;

public interface IPublisher
{
    public  Task PublishOperationEntryMessage(AdditionOperationDto dto);
}