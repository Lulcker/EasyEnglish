using EasyEnglish.Core.Domain;

namespace EasyEnglish.Domain.Entities;

/// <summary>
/// Пользователь
/// </summary>
public class User : EntityBase
{
    /// <summary>
    /// Имя
    /// </summary>
    public required string FirstName { get; set; }
    
    /// <summary>
    /// Почта
    /// </summary>
    public required string Email { get; set; }
    
    /// <summary>
    /// Хэш пароля
    /// </summary>
    public required string PasswordHash { get; set; }
    
    /// <summary>
    /// Соль пароля
    /// </summary>
    public required string PasswordSalt { get; set; }

    /// <summary>
    /// Коллекции карточек
    /// </summary>
    public ICollection<CardCollection> CardCollections { get; set; } = new HashSet<CardCollection>();

    /// <summary>
    /// Настройки напоминаний
    /// </summary>
    public UserReminderSettings? ReminderSettings { get; set; }
}