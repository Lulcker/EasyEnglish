namespace EasyEnglish.DTO.Cards.RequestModels;

/// <summary>
/// Входная модель для обновления карточки
/// </summary>
public class UpdateCardRequestModel
{
    /// <summary>
    /// Id карточки
    /// </summary>
    public required Guid Id { get; init; }
    
    /// <summary>
    /// Слово на русском
    /// </summary>
    public required string RuWord { get; init; }
    
    /// <summary>
    /// Слово на английском
    /// </summary>
    public required string EnWord { get; init; }
    
    /// <summary>
    /// Подтверждение действия
    /// </summary>
    public required bool IsConfirmAction { get; init; }
}