namespace Infrastructure;

public class OperationService
{
    public void LogOperation(string operationType, double operand1, double operand2, double result)
     {
         
         throw new Exception("Simulated failure");
    //     Console.WriteLine("am here ");
    //     using (var context = new OperationDbContext())
    //     {
    //         var operation = new Operation
    //         {
    //             OperationType = operationType,
    //             Operand1 = operand1,
    //             Operand2 = operand2,
    //             Result = result,
    //             Timestamp = DateTime.UtcNow
    //         };
    //         context.Operations.Add(operation);
    //         context.SaveChanges();
    //     }
    }
}