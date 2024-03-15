using EasyNetQ;
using OpenTelemetry;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Serilog;
using SubtractionAPI.services;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddSingleton<Subtraction>();

builder.Services.AddSingleton<IBus>(RabbitHutch.CreateBus("host=rabbitmq;virtualHost=/;username=user;password=pass"));

builder.Services.AddOpenTelemetry()
    .ConfigureResource(builder => builder.AddService("SubtractionService"))
    .WithTracing(builder =>
        {
            builder
                .AddZipkinExporter(options =>
                    options.Endpoint = new Uri("http://zipkin:9411/api/v2/spans"))
                .AddSource("SubtractionService.Subtraction")
                .SetSampler(new AlwaysOnSampler())
                .AddAspNetCoreInstrumentation()
                .AddConsoleExporter();
        }
    );

// using var tracerProvider = Sdk.CreateTracerProviderBuilder()
//     .SetSampler(new AlwaysOnSampler())
//     .AddSource("SubtractionService.Subtraction")
//     .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService("SubtractionService"))
//     .AddZipkinExporter(options =>
//     {
//         options.Endpoint = new Uri("http://zipkin:9411/api/v2/spans");
//     })
//     .Build();

        
// Serilog // http://127.0.0.1:5342/#/events?range=1d
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.Seq("http://seq:5341") 
    // setup
    .CreateLogger();

Log.Information("Starting SubtractionService");


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();