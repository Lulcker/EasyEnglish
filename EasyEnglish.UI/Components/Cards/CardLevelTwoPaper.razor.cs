using EasyEnglish.DTO.Cards.ResponseModels;
using Microsoft.AspNetCore.Components;

namespace EasyEnglish.UI.Components.Cards;

/// <summary>
/// Карточка для уровня 2
/// </summary>
public partial class CardLevelTwoPaper : ComponentBase
{
    #region Parameters

    /// <summary>
    /// Карточка
    /// </summary>
    [Parameter, EditorRequired]
    public required CardResponseModel Card { get; set; }
    
    /// <summary>
    /// Перевёрнута?
    /// </summary>
    [Parameter, EditorRequired]
    public required bool IsFlipped { get; set; }
    
    /// <summary>
    /// Событие переворота
    /// </summary>
    [Parameter, EditorRequired]
    public required EventCallback<bool> IsFlippedChanged { get; set; }

    #endregion

    #region Methods

    private async Task FlipCard() =>
        await IsFlippedChanged.InvokeAsync(!IsFlipped);

    #endregion
}