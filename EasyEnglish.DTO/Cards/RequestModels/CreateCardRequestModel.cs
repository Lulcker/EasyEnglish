namespace EasyEnglish.DTO.Cards.RequestModels;

/// <summary>
/// Входная модель для создания карточки
/// </summary>
public class CreateCardRequestModel
{
    /// <summary>
    /// Слово на русском
    /// </summary>
    public required string RuWord { get; init; }
    
    /// <summary>
    /// Слово на английском
    /// </summary>
    public required string EnWord { get; init; }
    
    /// <summary>
    /// Id коллекции карточек
    /// </summary>
    public required Guid CardCollectionId { get; init; }
    
    /// <summary>
    /// Подтверждение действия
    /// </summary>
    public required bool IsConfirmAction { get; init; }
}