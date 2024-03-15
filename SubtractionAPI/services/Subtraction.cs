using System;
using System.Diagnostics;
using Polly;
using Polly.CircuitBreaker;
using Polly.Retry;
using Serilog;
// for rabbit mq 
using RabbitMQ.Client;
using System.Text;
using EasyNetQ;
using EasyNetQ.DI;
using EasyNetQ.Serialization.SystemTextJson;
using SharedConfiguration;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace SubtractionAPI.services;

public class Subtraction : ISubtraction
    {
        private readonly IBus _bus;
        
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

        public Subtraction()
        {
            _bus = RabbitHutch.CreateBus("host=rabbitmq;virtualHost=/;username=user;password=pass", serviceRegister =>
                serviceRegister.Register<ISerializer>(_ => new SystemTextJsonSerializer()));
        }
        
        private void PublishOperationMessageQ(int number1, int number2, int result)
        {
            var message = new OperationEntryMessage
            { 
                OperationType = "Subtraction",
                Operand1 = number1,
                Operand2 = number2,
                Result = result,
                TimeStamp = DateTime.UtcNow 
            };
    
            _ = _bus.PubSub.PublishAsync(message); 
            Log.Information("Hello Subtraction");
            Log.Information($" [x] Sent message: {JsonSerializer.Serialize(message)}");
            Console.WriteLine($" [x] Sent message: {JsonSerializer.Serialize(message)}");
        }

        public int Subtract(int number1, int number2)
        {
            using (var activity = TelemetryActivitySource.Instance.StartActivity("Subtract"))
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
                    
                    PublishOperationMessageQ(number1, number2, result);
                });

                return result;
            }
        }
    }