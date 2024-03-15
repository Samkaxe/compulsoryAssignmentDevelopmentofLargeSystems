using AdditionAPI.services;
using EasyNetQ;
using OpenTelemetry;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// adding the service here 
builder.Services.AddSingleton<Addition>();

builder.Services.AddSingleton<IBus>(RabbitHutch.CreateBus("host=rabbitmq;virtualHost=/;username=user;password=pass"));

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


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();