using EasyEnglish.Application.Contracts.Providers;
using EasyEnglish.Application.Helpers;
using EasyEnglish.Application.Rules.Users;
using EasyEnglish.Domain.Entities;
using EasyEnglish.DTO.Dictionaries;
using EasyEnglish.DTO.Users.RequestModels;
using EasyEnglish.Persistence;
using Hangfire;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EasyEnglish.Application.Commands.Users;

/// <summary>
/// Команда обновления настроек напоминаний
/// </summary>
public class UpdateReminderSettingsCommand(
    IRepository<UserReminderSettings> reminderSettingsRepository,
    IUnitOfWork unitOfWork,
    IUserInfoProvider userInfoProvider,
    IBackgroundJobClient backgroundJobClient,
    ILogger<UpdateReminderSettingsCommand> logger
)
{
    public async Task ExecuteAsync(UpdateReminderSettingsRequestModel requestModel)
    {
        switch (requestModel.Mode)
        {
            case ReminderMode.Fix:
                // TODO: FixMode
                break;
            case ReminderMode.Periodicity:
                requestModel.StartWorkTime.HasValue
                    .ThrowIfInvalidCondition("Время начала напоминаний должно быть установлено");
                
                requestModel.EndWorkTime.HasValue
                    .ThrowIfInvalidCondition("Время окончания напоминаний должно быть установлено");
                
                (requestModel.StartWorkTime!.Value < requestModel.EndWorkTime!.Value)
                    .ThrowIfInvalidCondition("Время начала напоминаний должно быть меньше времени окончания");
                
                requestModel.PeriodicityTime.HasValue
                    .ThrowIfInvalidCondition("Периодичность работы напоминаний должна быть установлена");
                
                (requestModel.PeriodicityTime!.Value >= 2)
                    .ThrowIfInvalidCondition("Периодичность работы напоминаний должна быть не менее 2 часов");
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        
        var reminderSettings = await reminderSettingsRepository
            .SingleOrDefaultAsync(u => u.UserId == userInfoProvider.Id);
        
        (reminderSettings is not null)
            .ThrowIfInvalidCondition("Напоминания не настроены");

        reminderSettings!.Mode = requestModel.Mode;
        reminderSettings.ReminderTimes = null;
        reminderSettings.StartWorkTime = requestModel.Mode is ReminderMode.Periodicity
            ? requestModel.StartWorkTime
            : null;
        reminderSettings.EndWorkTime = requestModel.Mode is ReminderMode.Periodicity
            ? requestModel.EndWorkTime
            : null;
        reminderSettings.PeriodicityTime = requestModel.Mode is ReminderMode.Periodicity
            ? requestModel.PeriodicityTime
            : null;
        
        var delay = RecurrenceHelper.GetNextReminderDelay(reminderSettings);

        var backgroundJobId = backgroundJobClient
            .Schedule<SendReminderToUserRule>(x => x.ExecuteAsync(userInfoProvider.Id), delay);

        reminderSettings.BackgroundJobId = backgroundJobId;

        await unitOfWork.SaveChangesAsync();
        
        logger.LogInformation("Обновлены настройки напоминаний для пользователя c Email: {UserEmail} " +
                              "Следующее напоминание через {Delay}", userInfoProvider.Email, delay);
    }
}