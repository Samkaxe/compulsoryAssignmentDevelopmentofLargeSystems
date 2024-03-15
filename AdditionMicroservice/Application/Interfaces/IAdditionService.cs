using AdditionMicroservice.Application.DTOs;

namespace AdditionMicroservice.Application.Interfaces;

public interface IAdditionService
{
    decimal Add(AdditionOperationDto dto);
}