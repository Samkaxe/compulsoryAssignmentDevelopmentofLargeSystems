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

// using var tracerProvider = Sdk.CreateTracerProviderBuilder()
//     .SetSampler(new AlwaysOnSampler())
//     .AddSource("AdditionService.Addition")
//     .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService("AdditionService"))
//     .AddZipkinExporter(options =>
//     {
//         options.Endpoint = new Uri("http://localhost:9411/api/v2/spans"); // error here if your using linux switch  to http://localhost:9411 only 
//     })
//     .Build();

builder.Services.AddOpenTelemetry()
    .ConfigureResource(builder => builder.AddService("AdditionService"))
    .WithTracing(builder =>
        {
            builder
                .AddZipkinExporter(options =>
                    options.Endpoint = new Uri("http://zipkin:9411/api/v2/spans"))
                .AddSource("AdditionService.Addition")
                .SetSampler(new AlwaysOnSampler())
                .AddAspNetCoreInstrumentation()
                .AddConsoleExporter();
        }
    );


Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.Seq("http://seq:5341") 
    // setup
    .CreateLogger();

// // Serilog
// Log.Logger = new LoggerConfiguration()
//     .MinimumLevel.Debug()
//     .WriteTo.Seq("http://localhost:5341")  // error here if your using linux switch  to  // http://seq-server:5341 or replace to 5342 depend on your docker
//     // setup
//     .CreateLogger();

Log.Information("Starting AdditionService");

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

app.UseCors();

app.UseAuthorization();

app.MapControllers();

app.Run();