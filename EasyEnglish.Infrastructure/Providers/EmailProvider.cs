using EasyEnglish.Application.Contracts.Providers;

namespace EasyEnglish.Infrastructure.Providers;

/// <summary>
/// Провайдер для сервиса отправки писем
/// </summary>
public class EmailProvider : IEmailProvider
{
    /// <summary>
    /// Хост
    /// </summary>
    public required string Host { get; init; }
    
    /// <summary>
    /// Порт
    /// </summary>
    public required int Port { get; init; }
    
    /// <summary>
    /// Почта отправителя
    /// </summary>
    public required string EmailTo { get; init; }
    
    /// <summary>
    /// Пароль
    /// </summary>
    public required string Password { get; init; }
}