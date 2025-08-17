using EasyEnglish.Core.Domain;

namespace EasyEnglish.Domain.Entities;

/// <summary>
/// Карточка
/// </summary>
public class Card : EntityBase
{
    /// <summary>
    /// Слово на русском
    /// </summary>
    public required string RuWord { get; set; }
    
    /// <summary>
    /// Слово на английском
    /// </summary>
    public required string EnWord { get; set; }
    
    /// <summary>
    /// Дата добавления
    /// </summary>
    public required DateTime AddedAt { get; init; }
    
    /// <summary>
    /// Id коллекции карточек
    /// </summary>
    public required Guid CardCollectionId { get; init; }

    /// <summary>
    /// Коллекция карточек
    /// </summary>
    public CardCollection CardCollection { get; init; } = null!;
}