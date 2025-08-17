namespace EasyEnglish.DTO.Cards.ResponseModels;

/// <summary>
/// Модель карточки
/// </summary>
public class CardResponseModel
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
    public required string EnWord { get; set; }
    
    /// <summary>
    /// Дата добавления
    /// </summary>
    public required DateTime AddedAt { get; init; }
}