using EasyEnglish.Domain.Entities;
using EasyEnglish.DTO.Dictionaries;

namespace EasyEnglish.Application.Helpers;

public static class RecurrenceHelper
{
    public static TimeSpan GetNextReminderDelay(UserReminderSettings reminderSettings)
    {
        var utcNow = DateTime.UtcNow;
        var userTimeZone = TimeZoneInfo.FindSystemTimeZoneById(reminderSettings.TimeZoneId);
        var userNow = TimeZoneInfo.ConvertTimeFromUtc(utcNow, userTimeZone);
        
        switch (reminderSettings.Mode)
        {
            case ReminderMode.Fix:
                // TODO: FixMode
                throw new NotSupportedException();
            
            case ReminderMode.Periodicity:
                var nextLocal = userNow.AddHours(reminderSettings.PeriodicityTime!.Value);
                var endWorkTimeLocal = userNow.Date.Add(reminderSettings.EndWorkTime!.Value);

                DateTime nextUtc;

                if (nextLocal < endWorkTimeLocal)
                {
                    nextUtc = TimeZoneInfo.ConvertTimeToUtc(nextLocal, userTimeZone);
                }
                else
                {
                    var startWorkTimeLocal = userNow.Date.AddDays(1).Add(reminderSettings.StartWorkTime!.Value);
                    nextUtc = TimeZoneInfo.ConvertTimeToUtc(startWorkTimeLocal, userTimeZone);
                }

                return nextUtc - utcNow;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}