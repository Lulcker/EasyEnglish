using EasyEnglish.Core.Domain;
using EasyEnglish.DTO.Dictionaries;

namespace EasyEnglish.Domain.Entities;

/// <summary>
/// Настройки напоминаний пользователя
/// </summary>
public class UserReminderSettings : EntityBase
{
    /// <summary>
    /// Включены уведомления
    /// </summary>
    public required bool IsEnabled { get; set; }
    
    /// <summary>
    /// Мод напоминаний
    /// </summary>
    public required ReminderMode Mode { get; set; }
    
    /// <summary>
    /// Id зоны времени
    /// </summary>
    public required string TimeZoneId { get; init; }
    
    /// <summary>
    /// Время напоминаний (для 
    /// </summary>
    public ICollection<TimeSpan>? ReminderTimes { get; set; }
    
    /// <summary>
    /// Старт рабочего времени
    /// </summary>
    public TimeSpan? StartWorkTime { get; set; }
    
    /// <summary>
    /// Конец рабочего времени
    /// </summary>
    public TimeSpan? EndWorkTime { get; set; }
    
    /// <summary>
    /// Переодичность
    /// </summary>
    public int? PeriodicityTime { get; set; }
    
    /// <summary>
    /// Id переодической задачи напоминаний
    /// </summary>
    public string? BackgroundJobId { get; set; }
    
    /// <summary>
    /// Id пользователя
    /// </summary>
    public required Guid UserId { get; init; }

    /// <summary>
    /// Пользователь
    /// </summary>
    public User User { get; init; } = null!;
}