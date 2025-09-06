using EasyEnglish.DTO.Dictionaries;

namespace EasyEnglish.DTO.Users.ResponseModels;

/// <summary>
/// Модель настроек напоминаний
/// </summary>
public class ReminderSettingsResponseModel
{
    /// <summary>
    /// Включены ли настройки?
    /// </summary>
    public required bool IsEnabled { get; init; }
    
    /// <summary>
    /// Мод напоминаний
    /// </summary>
    public required ReminderMode Mode { get; init; }
    
    /// <summary>
    /// Время напоминаний
    /// </summary>
    public required ICollection<TimeSpan>? ReminderTimes { get; init; }
    
    /// <summary>
    /// Старт рабочего времени
    /// </summary>
    public required TimeSpan? StartWorkTime { get; init; }
    
    /// <summary>
    /// Конец рабочего времени
    /// </summary>
    public required TimeSpan? EndWorkTime { get; init; }
    
    /// <summary>
    /// Переодичность
    /// </summary>
    public required int? PeriodicityTime { get; init; }
}