namespace EasyEnglish.Configurators;

/// <summary>
/// Конфигурация логирования
/// </summary>
internal static class LoggingConfigurator
{
    internal static WebApplicationBuilder ConfigureLogging(this WebApplicationBuilder builder)
    {
        builder.Services.AddLogging(cfg => cfg.AddFilter("Microsoft.AspNetCore.Diagnostics.ExceptionHandlerMiddleware", LogLevel.None));
        
        return builder;
    }
}