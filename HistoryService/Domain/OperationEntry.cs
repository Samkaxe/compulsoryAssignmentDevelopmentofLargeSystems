namespace HistoryService.Domain;

public class OperationEntry
{
    public int Id { get; set; }
    public string OperationType { get; set; } // "Addition" or "Subtraction"
    public decimal Operand1 { get; set; }
    public decimal Operand2 { get; set; }
    public decimal Result { get; set; }
    public DateTime TimeStamp { get; set; }
}