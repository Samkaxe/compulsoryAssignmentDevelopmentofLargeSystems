using System.Diagnostics;

namespace SharedConfiguration;

public static class TelemetryActivitySource
{
    public static readonly ActivitySource Instance = new ActivitySource("Service");
}