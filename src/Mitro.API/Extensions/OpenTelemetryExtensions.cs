using OpenTelemetry;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace Mitro.API.Extensions;

internal static class OpenTelemetryExtensions
{
    internal static IHostApplicationBuilder ConfigureOpenTelemetry(
        this IHostApplicationBuilder builder)
    {
        builder
            .AddLogging()
            .AddMetrics()
            .AddTracing();

        return builder;
    }

    private static IHostApplicationBuilder AddLogging(
        this IHostApplicationBuilder builder)
    {
        builder.Logging.AddOpenTelemetry(loggerOptions =>
        {
            loggerOptions.IncludeFormattedMessage = true;
            loggerOptions.IncludeScopes = true;

            loggerOptions.AddOtlpExporter();
        });

        return builder;
    }

    private static OpenTelemetryBuilder AddMetrics(
        this IHostApplicationBuilder builder)
    {
        return builder.Services
            .AddOpenTelemetry()
            .ConfigureResource(configure => 
            {
                configure.AddService("Mitro.API");
            })
            .WithMetrics(metrics => 
            {
                metrics.AddOtlpExporter();
            });
    }

    private static OpenTelemetryBuilder AddTracing(
        this OpenTelemetryBuilder builder)
    {
        return builder
            .WithTracing(tracing => 
            {
                tracing.AddOtlpExporter();
            });
    }
}
