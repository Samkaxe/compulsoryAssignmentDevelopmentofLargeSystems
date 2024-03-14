using System;
using System.Diagnostics;
using Polly;
using Polly.CircuitBreaker;
using Infrastructure;
using Polly.Retry;
using Serilog;

namespace AdditionService
{
    public class Addition : IAddition
    {
        private static readonly ActivitySource ActivitySource = new("AdditionService.Addition");
        private static readonly OperationService OperationService = new OperationService();

       
        private static readonly RetryPolicy retryPolicy = Policy
            .Handle<Exception>() // i added mock exception in the database service in the infrastructure 
            .Retry(3, onRetry: (exception, retryCount) =>
            {
                Log.Warning($"Attempt {retryCount}: Retrying due to {exception.Message}");
            });

        private static readonly CircuitBreakerPolicy circuitBreakerPolicy = Policy
            .Handle<Exception>()
            .CircuitBreaker(2, TimeSpan.FromMinutes(1), 
                onBreak: (exception, breakDelay) =>
                {
                    Log.Warning($"Circuit broken due to {exception.Message}, circuit open for {breakDelay.TotalSeconds}s");
                },
                onReset: () =>
                {
                    Log.Information("Circuit reset.");
                });

        public int Add(int number1, int number2)
        {
            using (var activity = ActivitySource.StartActivity("Add"))
            {
                if (activity == null)
                {
                    Log.Warning("Activity was not created.");
                    return number1 + number2;
                }

                // activity.SetTag("input.number1", number1);
                // activity.SetTag("input.number2", number2);

                int result = number1 + number2;
                // activity.SetTag("output.result", result);
                
                var policyWrap = Policy.Wrap(retryPolicy, circuitBreakerPolicy);

                policyWrap.Execute(() => 
                {
                    OperationService.LogOperation("Addition", number1, number2, result);
                });

                Log.Information($"Addition result: {result}");
                return result;
            }
        }
    }
}
