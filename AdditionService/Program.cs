using AdditionService;
using Serilog;
using System;
using Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using OpenTelemetry;
using OpenTelemetry.Trace;
using OpenTelemetry.Resources;

class Program
{
    static void Main(string[] args)
    {
        using var tracerProvider = Sdk.CreateTracerProviderBuilder()
            .SetSampler(new AlwaysOnSampler())
            .AddSource("AdditionService.Addition")
            .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService("AdditionService"))
            .AddZipkinExporter(options =>
            {
                options.Endpoint = new Uri("http://localhost:9411/api/v2/spans"); // error here if your using linux switch  to http://localhost:9411 only 
            })
            .Build();

        
        // Serilog
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.Seq("http://localhost:5341")  // error here if your using linux switch  to  // http://seq-server:5341 or replace to 5342 depend on your docker
            // setup
            .CreateLogger();

        Log.Information("Starting AdditionService");

        
        try
        {
            IAddition additionService = new Addition();
            Console.WriteLine("Enter the first number:");
            int number1 = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Enter the second number:");
            int number2 = Convert.ToInt32(Console.ReadLine());

            int result = additionService.Add(number1, number2);
            Console.WriteLine($"The result of addition is: {result}");

            Log.Information($"Added {number1} and {number2} to get {result}");
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "AdditionService terminated unexpectedly.");
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }
    
}