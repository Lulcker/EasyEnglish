namespace EasyEnglish.DTO.Users.RequestModels;

/// <summary>
/// Входная модель для переключения работы напоминаний
/// </summary>
public class ChangeEnabledReminderSettingsRequestModel
{
    /// <summary>
    /// Включены ли напоминания
    /// </summary>
    public required bool IsEnabled { get; init; }
}