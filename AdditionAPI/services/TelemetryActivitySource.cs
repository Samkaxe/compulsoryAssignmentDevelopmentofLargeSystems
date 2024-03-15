using System.Diagnostics;

namespace AdditionAPI.services;

public static class TelemetryActivitySource
{
    public static readonly ActivitySource Instance = new ActivitySource("SubtractionService.Subtraction");
}