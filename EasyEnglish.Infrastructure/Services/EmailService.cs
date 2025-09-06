using System.Net;
using System.Net.Mail;
using EasyEnglish.Application.Contracts.Providers;
using EasyEnglish.Application.Contracts.Services;
using Microsoft.Extensions.Logging;

namespace EasyEnglish.Infrastructure.Services;

/// <summary>
/// Сервис отправки сообщений
/// </summary>
public class EmailService(
    IEmailProvider emailProvider,
    ILogger<EmailService> logger
    ) : IEmailService
{
    public async Task SendEmailAsync(string email, string subject, string message, bool isBodyHtml)
    {
        using var smtpClient = new SmtpClient(emailProvider.Host, emailProvider.Port);
        
        smtpClient.Credentials = new NetworkCredential(emailProvider.EmailTo, emailProvider.Password);
        smtpClient.EnableSsl = true;

        var mailMessage = new MailMessage(emailProvider.EmailTo, email, subject, message)
        {
            IsBodyHtml = isBodyHtml
        };

        try
        {
            await smtpClient.SendMailAsync(mailMessage);
        }
        catch (Exception e)
        {
            logger.LogError("Не удалось отправить сообщение пользователю с Email: {UserEmail}, Ошибка: {ErrorMessage}",
                email, e.Message);
        }
    }
}