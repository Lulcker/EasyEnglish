namespace EasyEnglish.Application.Contracts.Providers;

/// <summary>
/// Провайдер для сервиса отправки писем
/// </summary>
public interface IEmailProvider
{
    /// <summary>
    /// Хост
    /// </summary>
    string Host { get; }
    
    /// <summary>
    /// Порт
    /// </summary>
    int Port { get; }
    
    /// <summary>
    /// Почта отправителя
    /// </summary>
    string EmailTo { get; }
    
    /// <summary>
    /// Пароль
    /// </summary>
    string Password { get; }
}