namespace EasyEnglish.DTO.CardCollections.ResponseModels;

/// <summary>
/// Модель коллекции карточек
/// </summary>
public class CardCollectionResponseModel
{
    /// <summary>
    /// Id коллекции
    /// </summary>
    public required Guid Id { get; init; }

    /// <summary>
    /// Название
    /// </summary>
    public required string Title { get; set; }
    
    /// <summary>
    /// Дата создания
    /// </summary>
    public required DateTime CreatedAt { get; init; }
    
    /// <summary>
    /// Количество карточек
    /// </summary>
    public required int CardsCount { get; init; }
}