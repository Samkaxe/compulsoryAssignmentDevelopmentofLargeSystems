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

namespace AdditionAPI.services;

 public class Addition : IAddition
    {
        private readonly IBus _bus;
       
        
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

        public Addition()
        {
            _bus = RabbitHutch.CreateBus("host=rabbitmq;virtualHost=/;username=user;password=pass", serviceRegister =>
                serviceRegister.Register<ISerializer>(_ => new SystemTextJsonSerializer()));
        }
        
        private void PublishOperationMessageQ(int number1, int number2, int result)
        {
            var message = new OperationEntryMessage
            { 
                OperationType = "Addition",
                Operand1 = number1,
                Operand2 = number2,
                Result = result,
                TimeStamp = DateTime.UtcNow 
            };
    
            _ = _bus.PubSub.PublishAsync(message); 
            Log.Information("Hello world");
            Log.Information($" [x] Sent message: {JsonSerializer.Serialize(message)}");
            Console.WriteLine($" [x] Sent message: {JsonSerializer.Serialize(message)}");
        }
        
        
        public  int Add(int number1, int number2)
        {
            
            using (var activity = TelemetryActivitySource.Instance.StartActivity("Add"))
            {
                if (activity == null)
                {
                    Log.Warning("Activity was not created.");
                    return number1 + number2;
                }

                activity.SetTag("input.number1", number1);
                activity.SetTag("input.number2", number2);

                int result = number1 + number2;
                activity.SetTag("output.result", result);
                
                var policyWrap = Policy.Wrap(retryPolicy, circuitBreakerPolicy);

                policyWrap.Execute(() => 
                {
                    
                   // PublishOperationMessage(number1, number2, result);
                      PublishOperationMessageQ(number1, number2, result);
                    //   OperationService.LogOperation("Addition", number1, number2, result);
                });
                
                Log.Information($"Addition result: {result}");
                return result;
            }
        }
        
        // private void PublishOperationMessage(int number1, int number2, int result)
        // {
        //     var factory = new ConnectionFactory() { HostName = "localhost" };
        //     using (var connection = factory.CreateConnection())
        //     using (var channel = connection.CreateModel())
        //     {
        //         channel.ExchangeDeclare(exchange: "operations_exchange", type: "topic");
        //
        //         var routingKey = "addition.operation.logged";
        //         var message = new { Number1 = number1, Number2 = number2, Result = result };
        //         var messageBody = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));
        //
        //         channel.BasicPublish(exchange: "operations_exchange",
        //             routingKey: routingKey,
        //             basicProperties: null,
        //             body: messageBody);
        //         Console.WriteLine(" [x] Sent '{0}':'{1}'", routingKey, message);
        //     }
        // }
    }