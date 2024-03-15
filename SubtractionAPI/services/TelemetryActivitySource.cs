using System.Diagnostics;

namespace SubtractionAPI.services;

public static class TelemetryActivitySource
{
    public static readonly ActivitySource Instance = new ActivitySource("SubtractionService.Subtraction");
}