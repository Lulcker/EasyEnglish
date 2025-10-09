using EasyEnglish.Application.Contracts.Providers;
using EasyEnglish.Application.Contracts.Services;
using EasyEnglish.Application.Helpers;
using EasyEnglish.Domain.Entities;
using EasyEnglish.Persistence;
using Hangfire;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EasyEnglish.Application.Rules.Users;

/// <summary>
/// Правило отправки напоминаний
/// </summary>
public class SendReminderToUserRule(
    IRepository<User> userRepository,
    IUnitOfWork unitOfWork,
    IEmailService emailService,
    IAesCryptoService aesCryptoService,
    IBackgroundJobClient backgroundJobClient,
    IUrlUIProvider urlUiProvider,
    ILogger<SendReminderToUserRule> logger
    )
{
    #region Consts

    private const string EmailSubject = "Пора повторить слова!";

    private const string EmailMessage = """
                                        <!doctype html>
                                        <html lang="ru" xmlns="http://www.w3.org/1999/xhtml">
                                        <head>
                                          <meta charset="utf-8">
                                          <meta name="viewport" content="width=device-width">
                                          <meta http-equiv="x-ua-compatible" content="ie=edge">
                                          <title>Пора повторить слова!</title>
                                          <style>
                                            body { margin:0; padding:0; background:#ffffff; }
                                            table { border-collapse:collapse; }
                                            img { border:0; line-height:100%; vertical-align:middle; }
                                            .wrapper { width:100%; background:#ffffff; padding:24px 0; }
                                            .container { width:100%; max-width:560px; margin:0 auto; background:#ffffff; }
                                            .padded { padding:24px; }
                                            .brand { font:700 20px/1.3 -apple-system,BlinkMacSystemFont,Segoe UI,Roboto,Arial,sans-serif; color:#111; }
                                            .h1 { font:700 22px/1.35 -apple-system,BlinkMacSystemFont,Segoe UI,Roboto,Arial,sans-serif; color:#111; margin:0 0 12px; }
                                            .text { font:400 16px/1.6 -apple-system,BlinkMacSystemFont,Segoe UI,Roboto,Arial,sans-serif; color:#333; margin:0 0 16px; }
                                            .btn-wrap { text-align:center; padding:8px 0 4px; }
                                            .btn {
                                              display:inline-block; text-decoration:none; font:600 16px/1 -apple-system,BlinkMacSystemFont,Segoe UI,Roboto,Arial,sans-serif;
                                              padding:14px 22px; border-radius:8px; background:#274edb; color:#fff;
                                            }
                                            .foot { font:400 12px/1.5 -apple-system,BlinkMacSystemFont,Segoe UI,Roboto,Arial,sans-serif; color:#777; }
                                          </style>
                                        </head>
                                        <body>
                                          <center class="wrapper">
                                            <!-- Preheader -->
                                            <div style="display:none; font-size:1px; color:#fff; line-height:1px; max-height:0; max-width:0; opacity:0; overflow:hidden;">
                                              Зайдите на EasyEnglish и повторите слова — это займет пару минут.
                                            </div>
                                        
                                            <table role="presentation" class="container" cellpadding="0" cellspacing="0">
                                              <tr>
                                                <td class="padded">
                                                  <h1 class="h1">Пора повторить слова!</h1>
                                        
                                                  <p class="text">
                                                    Не откладывайте на потом: откройте тренировку и повторите новые слова.
                                                    Всего пару минут — и заметный прогресс уже сегодня.
                                                  </p>
                                        
                                                  <div class="btn-wrap">
                                                    <a class="btn" href="${link}">
                                                      Перейти к повторению
                                                    </a>
                                                  </div>
                                        
                                                  <p class="text" style="margin-top:18px;">
                                                    Если кнопка не работает, скопируйте ссылку и вставьте в браузер:<br>
                                                    <a href="${link}"
                                                       style="color:#274edb; word-break:break-all;">
                                                      ${link}
                                                    </a>
                                                  </p>
                                        
                                                  <p class="foot">
                                                    Вы получили это письмо, потому что включили напоминания в профиле EasyEnglish.
                                                  </p>
                                                </td>
                                              </tr>
                                            </table>
                                          </center>
                                        </body>
                                        </html>
                                        """;

    #endregion
    
    public async Task ExecuteAsync(Guid userId)
    {
        var user = await userRepository
            .Include(u => u.ReminderSettings)
            .SingleAsync(u => u.Id == userId);
        
        var email = aesCryptoService.Decrypt(user.Email);
        
        await emailService.SendEmailAsync(
          email: email,
          subject: EmailSubject,
          message: EmailMessage.Replace("${link}", urlUiProvider.Url),
          isBodyHtml: true);
        
        var delay = RecurrenceHelper.GetNextReminderDelay(user.ReminderSettings!);
        
        var backgroundJobId = backgroundJobClient
            .Schedule<SendReminderToUserRule>(x => ExecuteAsync(userId), delay);
        
        user.ReminderSettings!.BackgroundJobId = backgroundJobId;
        
        await unitOfWork.SaveChangesAsync();
        
        logger.LogInformation("Отправлено напоминание пользователю с Email: {UserEmail} (Id: {UserId}) " +
                              "Следующее напоминание через {Delay}", email, userId, delay);
    }
}