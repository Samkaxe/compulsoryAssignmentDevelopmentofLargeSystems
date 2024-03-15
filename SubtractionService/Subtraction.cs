using System;
using System.Diagnostics;
using System.Text;
using System.Text.Json;
using OpenTelemetry;
using Polly;
using Polly.CircuitBreaker;
using Polly.Retry;
using RabbitMQ.Client;
using Serilog;

namespace SubtractionService
{
    public class Subtraction : ISubtraction
    {
        private static readonly ActivitySource ActivitySource = new("SubtractionService.Subtraction");
        //private static readonly OperationService OperationService = new OperationService(); we dont need this anymore 
        
        private static readonly RetryPolicy retryPolicy = Policy
            .Handle<Exception>()
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

        private void PublishOperationMessage(int number1, int number2, int result)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using(var connection = factory.CreateConnection())
            using(var channel = connection.CreateModel())
            {
                channel.ExchangeDeclare(exchange: "operations_exchange", type: "topic");

                var routingKey = "subtraction.operation.logged";
                var message = new { Number1 = number1, Number2 = number2, Result = result };
                var messageBody = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));

                channel.BasicPublish(exchange: "operations_exchange",
                                     routingKey: routingKey,
                                     basicProperties: null,
                                     body: messageBody);
                Console.WriteLine(" [x] Sent '{0}':'{1}'", routingKey, message);
            }
        }

        public int Subtract(int number1, int number2)
        {
            using (var activity = ActivitySource.StartActivity("Subtract"))
            {
                if (activity == null)
                {
                    Console.WriteLine("Activity was not created. Tracing might be misconfigured.");
                    return number1 - number2;
                }

                activity.SetTag("input.number1", number1);
                activity.SetTag("input.number2", number2);

                int result = number1 - number2;
                activity.SetTag("output.result", result);

                var policyWrap = Policy.Wrap(retryPolicy, circuitBreakerPolicy);

                policyWrap.Execute(() => 
                {
                    
                    PublishOperationMessage(number1, number2, result);
                });

                return result;
            }
        }
    }
}