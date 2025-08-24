using EasyEnglish.Core.Domain;

namespace EasyEnglish.Domain.Entities;

/// <summary>
/// Коллекция карточек
/// </summary>
public class CardCollection : EntityBase
{
    /// <summary>
    /// Название
    /// </summary>
    public required string Title { get; set; }
    
    /// <summary>
    /// Дата создания
    /// </summary>
    public required DateTime CreatedAt { get; init; }
    
    /// <summary>
    /// Id пользователя
    /// </summary>
    public required Guid UserId { get; init; }

    /// <summary>
    /// Пользователь
    /// </summary>
    public User User { get; init; } = null!;

    /// <summary>
    /// Карточки
    /// </summary>
    public ICollection<Card> Cards { get; set; } = new HashSet<Card>();
}