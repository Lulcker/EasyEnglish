using EasyEnglish.ProxyApiMethods;
using EasyEnglish.UI.Contracts;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

namespace EasyEnglish.UI.Configurators;

/// <summary>
/// Конфигурация обработчика ошибок
/// </summary>
internal static class ExceptionHandlerConfigurator
{
    internal static WebAssemblyHostBuilder ConfigureExceptionHandler(this WebAssemblyHostBuilder builder)
    {

        builder.Logging.ClearProviders();

        builder.Services.AddSingleton<ILoggerProvider, ExceptionProvider>();
        
        return builder;
    }
}

internal class ExceptionProvider(
    IUserSession userSession,
    ISnackbarHelper snackbarHelper
) : ILoggerProvider
{
    public ILogger CreateLogger(string categoryName) => new ExceptionHandler(userSession, snackbarHelper);
    
    public void Dispose() => GC.SuppressFinalize(this);
}

public class ExceptionHandler(
    IUserSession userSession,
    ISnackbarHelper snackbarHelper
) : ILogger
{
    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {
        switch (exception)
        {
            case AccessDeniedException:
                _ = userSession.EndSession();
                break;
            case InternalServerErrorException:
            case BusinessException:
            case ConfirmActionException:
                _ = snackbarHelper.ShowError(exception.Message);
                break;
        }
    }

    public bool IsEnabled(LogLevel logLevel)
    {
        return true;
    }

    public IDisposable? BeginScope<TState>(TState state) where TState : notnull
    {
        return null;
    }
}