using EasyEnglish.DTO.Dictionaries;

namespace EasyEnglish.DTO.Cards.ResponseModels;

/// <summary>
/// Модель карточки для теста
/// </summary>
public sealed class CardForTestResponseModel : CardResponseModel
{
    /// <summary>
    /// Уровень
    /// </summary>
    public CardLevel Level { get; set; } = CardLevel.One;
}