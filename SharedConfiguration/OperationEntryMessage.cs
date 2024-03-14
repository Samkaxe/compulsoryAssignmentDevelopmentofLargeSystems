namespace SharedConfiguration;

public class OperationEntryMessage
{
    public string OperationType { get; set; }
    public decimal Operand1 { get; set; }
    public decimal Operand2 { get; set; }
    public decimal Result { get; set; }
    
    public DateTime TimeStamp { get; set; }
}