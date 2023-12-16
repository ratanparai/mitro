using OpenTelemetry;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
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
            .WithMetrics(metrics => 
            {
                metrics.AddAspNetCoreInstrumentation();
                metrics.AddHttpClientInstrumentation();
                metrics.AddRuntimeInstrumentation();

                metrics.AddOtlpExporter();
            });
    }

    private static OpenTelemetryBuilder AddTracing(
        this OpenTelemetryBuilder builder)
    {
        return builder
            .WithTracing(tracing => 
            {
                tracing.AddAspNetCoreInstrumentation();
                tracing.AddHttpClientInstrumentation();

                tracing.AddOtlpExporter();
            });
    }
}
