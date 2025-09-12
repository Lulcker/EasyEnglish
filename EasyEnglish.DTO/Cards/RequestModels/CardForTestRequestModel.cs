namespace EasyEnglish.DTO.Cards.RequestModels;

/// <summary>
/// Входная модель для получения карточек для теста
/// </summary>
public class CardForTestRequestModel
{
    /// <summary>
    /// Id коллекции
    /// </summary>
    public required Guid? CardCollectionId { get; init; }
    
    /// <summary>
    /// Использовать ответы с выбором
    /// </summary>
    public required bool UseAnswerChoice { get; init; }
    
    /// <summary>
    /// Использовать ответы с вводом
    /// </summary>
    public required bool UseAnswerWriting { get; init; }
}