namespace AdditionMicroservice.Application.DTOs;

public class AdditionOperationDto
{
    public decimal Operand1 { get; set; }
    public decimal Operand2 { get; set; }
    public DateTime TimeStamp { get; set; }
}