namespace EasyEnglish.DTO.CardCollections.RequestModels;

/// <summary>
/// Входная модель для создания коллекции карточек
/// </summary>
public class CreateCardCollectionRequestModel
{
    /// <summary>
    /// Название
    /// </summary>
    public required string Title { get; init; }
}