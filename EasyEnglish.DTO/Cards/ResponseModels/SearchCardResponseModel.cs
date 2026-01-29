namespace EasyEnglish.DTO.Cards.ResponseModels;

/// <summary>
/// Модель карточки в поиске
/// </summary>
public class SearchCardResponseModel
{
    /// <summary>
    /// Id карточки
    /// </summary>
    public required Guid Id { get; init; }
    
    /// <summary>
    /// Слово на русском
    /// </summary>
    public required string RuWord { get; set; }
    
    /// <summary>
    /// Слово на английском
    /// </summary>
    public required string EnWord { get; init; }
    
    /// <summary>
    /// Id коллекции карточек
    /// </summary>
    public required Guid CardCollectionId { get; init; }
    
    /// <summary>
    /// Название коллекции карточек
    /// </summary>
    public required string CardCollectionTitle { get; init; }
}