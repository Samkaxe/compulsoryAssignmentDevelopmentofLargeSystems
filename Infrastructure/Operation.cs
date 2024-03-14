namespace Infrastructure;

public class Operation
{
    public int Id { get; set; }
    public string OperationType { get; set; }
    public double Operand1 { get; set; }
    public double Operand2 { get; set; }
    public double Result { get; set; }
    public DateTime Timestamp { get; set; }
}