using EasyEnglish.Application.Contracts.Providers;
using EasyEnglish.Domain.Entities;
using EasyEnglish.DTO.Users.ResponseModels;
using EasyEnglish.Persistence;
using Microsoft.EntityFrameworkCore;

namespace EasyEnglish.Application.Queries.Users;

/// <summary>
/// Запрос получения напоминаний пользователя
/// </summary>
public class GetReminderSettingsQuery(
    IRepository<User> userRepository,
    IUserInfoProvider userInfoProvider
    )
{
    public async Task<ReminderSettingsResponseModel?> ExecuteAsync(CancellationToken cancellationToken)
    {
        var user = await userRepository
            .AsNoTracking()
            .Where(u => u.Id == userInfoProvider.Id)
            .Select(u => new
            {
                u.ReminderSettings
            })
            .SingleAsync(cancellationToken);

        if (user.ReminderSettings is null)
            return null;

        return new ReminderSettingsResponseModel
        {
            IsEnabled = user.ReminderSettings.IsEnabled,
            Mode = user.ReminderSettings.Mode,
            ReminderTimes = user.ReminderSettings.ReminderTimes,
            StartWorkTime = user.ReminderSettings.StartWorkTime,
            EndWorkTime = user.ReminderSettings.EndWorkTime,
            PeriodicityTime = user.ReminderSettings.PeriodicityTime
        };
    }
}