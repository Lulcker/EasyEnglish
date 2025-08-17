namespace EasyEnglish.DTO.CardCollections.RequestModels;

/// <summary>
/// Входная модель для обновления коллекции карточек
/// </summary>
public class UpdateCardCollectionRequestModel : CreateCardCollectionRequestModel
{
    /// <summary>
    /// Id коллекции
    /// </summary>
    public required Guid Id { get; init; }
}