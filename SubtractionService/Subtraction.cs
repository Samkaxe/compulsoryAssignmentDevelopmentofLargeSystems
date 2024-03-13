using System;
using System.Diagnostics;
using Infrastructure;
using OpenTelemetry;

namespace SubtractionService
{
    public class Subtraction : ISubtraction
    {
        private static readonly ActivitySource ActivitySource = new("SubtractionService.Subtraction");
        public static OperationService operationService = new OperationService();
        public int Subtract(int number1, int number2)
        {
            using (var activity = ActivitySource.StartActivity("Subtract"))
            {
                if (activity == null)
                {
                    Console.WriteLine("Activity was not created. Tracing might be misconfigured.");
                    return number1 - number2;
                }

                activity.SetTag("i love single moms " , "");
                activity.SetTag("input.number1", number1);
                activity.SetTag("input.number2", number2);
                
                int result = number1 - number2;
                activity.SetTag("output.result", result);
                operationService.LogOperation("Subtraction", number1, number2, result);

                return result;
            }
        }
    }
}