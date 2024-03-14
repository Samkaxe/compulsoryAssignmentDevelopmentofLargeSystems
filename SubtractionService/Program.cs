using SubtractionService;
using Serilog;
using System;
using OpenTelemetry;
using OpenTelemetry.Trace;
using OpenTelemetry.Resources;

class Program
{
    static void Main(string[] args)
    {
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.Console()
            .WriteTo.Seq("http://localhost:5341")
            .CreateLogger();
        
        using var tracerProvider = Sdk.CreateTracerProviderBuilder()
            .SetSampler(new AlwaysOnSampler())
            .AddSource("SubtractionService.Subtraction")
            .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService("SubtractionService"))
            .AddZipkinExporter(options =>
            {
                options.Endpoint = new Uri("http://localhost:9411/api/v2/spans");
            })
            .Build();

        Log.Information("Starting SubtractionService");

        try
        {
            ISubtraction subtractionService = new Subtraction();
            Console.WriteLine("Enter the first number:");
            int number1 = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Enter the second number:");
            int number2 = Convert.ToInt32(Console.ReadLine());

            int result = subtractionService.Subtract(number1, number2);
            Console.WriteLine($"The result of subtraction is: {result}");

            Log.Information($"Subtracted {number2} from {number1} to get {result}");
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "SubtractionService terminated unexpectedly.");
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }
}