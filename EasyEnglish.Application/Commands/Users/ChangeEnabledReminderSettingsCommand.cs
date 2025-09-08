using EasyEnglish.Application.Contracts.Providers;
using EasyEnglish.Application.Helpers;
using EasyEnglish.Application.Rules.Users;
using EasyEnglish.Domain.Entities;
using EasyEnglish.DTO.Users.RequestModels;
using EasyEnglish.Persistence;
using Hangfire;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EasyEnglish.Application.Commands.Users;

/// <summary>
/// Команда переключения работы напоминаний
/// </summary>
public class ChangeEnabledReminderSettingsCommand(
    IRepository<UserReminderSettings> userReminderSettingsRepository,
    IUnitOfWork unitOfWork,
    IUserInfoProvider userInfoProvider,
    IBackgroundJobClient backgroundJobClient,
    ILogger<ChangeEnabledReminderSettingsCommand> logger
    )
{
    public async Task ExecuteAsync(ChangeEnabledReminderSettingsRequestModel requestModel)
    {
        var reminderSettings = await userReminderSettingsRepository
            .SingleOrDefaultAsync(r => r.UserId == userInfoProvider.Id);
        
        (reminderSettings is not null)
            .ThrowIfInvalidCondition("Напоминания не настроены");

        if (requestModel.IsEnabled)
        {
            (!reminderSettings!.IsEnabled)
                .ThrowIfInvalidCondition("Напоминания уже включены");
            
            var delay = RecurrenceHelper.GetNextReminderDelay(reminderSettings);
            
            var backgroundJobId = backgroundJobClient
                .Schedule<SendReminderToUserRule>(x => x.ExecuteAsync(userInfoProvider.Id), delay);

            reminderSettings.BackgroundJobId = backgroundJobId;
        }
        else
        {
            reminderSettings!.IsEnabled
                .ThrowIfInvalidCondition("Напоминания уже выключены");

            backgroundJobClient.Delete(reminderSettings.BackgroundJobId);
            reminderSettings.BackgroundJobId = null;
        }

        reminderSettings.IsEnabled = requestModel.IsEnabled;

        await unitOfWork.SaveChangesAsync();

        if (requestModel.IsEnabled)
            logger.LogInformation("Включены напоминания для пользователя c Email: {UserEmail} (Id: {UserId})",
                userInfoProvider.Email, userInfoProvider.Id);
        else
            logger.LogInformation("Выключены напоминания для пользователя c Email: {UserEmail} (Id: {UserId})",
                userInfoProvider.Email, userInfoProvider.Id);
    }
}