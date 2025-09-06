namespace EasyEnglish.Application.Contracts.Services;

/// <summary>
/// Сервис почты
/// </summary>
public interface IEmailService
{
    /// <summary>
    /// Отправить сообщение
    /// </summary>
    /// <param name="email">Почта</param>
    /// <param name="subject">Субъект</param>
    /// <param name="message">Сообщение</param>
    /// <param name="isBodyHtml">Содержимое HTML?</param>
    Task SendEmailAsync(string email, string subject, string message, bool isBodyHtml);
}