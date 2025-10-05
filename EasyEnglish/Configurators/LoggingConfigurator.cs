using Serilog;
using Serilog.Events;
using Serilog.Filters;
using Serilog.Sinks.Grafana.Loki;

namespace EasyEnglish.Configurators;

/// <summary>
/// Конфигурация логирования
/// </summary>
internal static class LoggingConfigurator
{
    internal static WebApplicationBuilder ConfigureLogging(this WebApplicationBuilder builder)
    {
        var lokiUri = builder.Configuration.GetValue<string>("Loki:Uri");
        
        ArgumentException.ThrowIfNullOrWhiteSpace(lokiUri);
        
        Log.Logger = new LoggerConfiguration()
            .Enrich.FromLogContext()
            .Enrich.WithProperty("Environment", builder.Environment.EnvironmentName)
            .MinimumLevel.Information()
            .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
            .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Warning)
            .MinimumLevel.Override("Hangfire", LogEventLevel.Warning)
            .Filter.ByExcluding(Matching.FromSource("Microsoft.AspNetCore.Diagnostics.ExceptionHandlerMiddleware"))
            .WriteTo.Console()
            .WriteTo.GrafanaLoki(lokiUri, [
                new LokiLabel { Key = "app", Value = "EasyEnglish" }
            ])
            .CreateLogger();

        builder.Host.UseSerilog();
        
        return builder;
    }
}