using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Trace;

namespace Mitro.API.OpenTelemetry;

public static class WebApplicationBuilderExtensions
{
    public static IHostApplicationBuilder ConfigureOpenTelemetry(
        this IHostApplicationBuilder builder)
    {

        builder.Logging.AddOpenTelemetry(o =>
        {
            o.IncludeFormattedMessage = true;
            o.IncludeScopes = true;
        });

        builder.AddOpenTelemetryExporters();

        return builder;
    }

    private static IHostApplicationBuilder AddOpenTelemetryExporters(
        this IHostApplicationBuilder builder)
    {
        builder.Services.Configure<OpenTelemetryLoggerOptions>(logging => 
        {
            logging.AddOtlpExporter();
        });

        builder.Services.ConfigureOpenTelemetryTracerProvider(tracing => 
        {
            tracing.AddOtlpExporter();
        });

        builder.Services.ConfigureOpenTelemetryMeterProvider(metrics => 
        {
            metrics.AddOtlpExporter();
        });

        return builder;
    }
}
