namespace EasyEnglish.DTO.Cards.RequestModels;

/// <summary>
/// Входная модель для переноса карточки в другую коллекцию
/// </summary>
public class MoveCardRequestModel
{
    /// <summary>
    /// Id карточки
    /// </summary>
    public required Guid CardId { get; set; }
    
    /// <summary>
    /// Id коллекции
    /// </summary>
    public required Guid CardCollectionId { get; set; }
}