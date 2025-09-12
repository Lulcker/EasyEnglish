using EasyEnglish.DTO.Cards.ResponseModels;
using Microsoft.AspNetCore.Components;

namespace EasyEnglish.UI.Components.Cards;

/// <summary>
/// Карточка для уровня 3
/// </summary>
public partial class CardLevelThreePaper : ComponentBase
{
    #region Parameters

    /// <summary>
    /// Карточка
    /// </summary>
    [Parameter, EditorRequired]
    public required CardResponseModel Card { get; set; }
    
    /// <summary>
    /// Показывать текущую позицию
    /// </summary>
    [Parameter]
    public bool ShowCurrentIndex { get; set; }
    
    /// <summary>
    /// Текущий индекс карточки
    /// </summary>
    [Parameter] 
    public int Index { get; set; }
    
    /// <summary>
    /// Количество карточек
    /// </summary>
    [Parameter]
    public int CardsCount { get; set; }
    
    /// <summary>
    /// Событие при неправильном ответе
    /// </summary>
    [Parameter]
    public EventCallback OnWrongAnswer { get; set; }
    
    /// <summary>
    /// Событие при правильном ответе
    /// </summary>
    [Parameter]
    public EventCallback OnRightAnswer { get; set; }
    
    /// <summary>
    /// Событие при данном ответе
    /// </summary>
    [Parameter]
    public EventCallback OnGivenAnswer { get; set; }
    
    /// <summary>
    /// Только для чтения
    /// </summary>
    [Parameter]
    public bool ReadOnly { get; set; }

    /// <summary>
    /// Показывать ответ
    /// </summary>
    [Parameter] 
    public bool ShowAnswer { get; set; } = true;

    #endregion

    #region Fields

    /// <summary>
    /// Ответ
    /// </summary>
    private string answer = string.Empty;

    /// <summary>
    /// Правильный ответ?
    /// </summary>
    private bool? isCorrectAnswer;

    #endregion

    #region Methods

    public void ClearAnswer()
    {
        answer = string.Empty;
        isCorrectAnswer = null;
    }

    private async Task CheckAnswer()
    {
        var resultEquals = string.Equals(answer.Trim(), Card.EnWord, StringComparison.CurrentCultureIgnoreCase);

        isCorrectAnswer = resultEquals;

        if (resultEquals)
            await OnRightAnswer.InvokeAsync();
        else
            await OnWrongAnswer.InvokeAsync();

        await OnGivenAnswer.InvokeAsync();
    }
    
    private string GetTextFieldClass()
    {
        if (!isCorrectAnswer.HasValue)
            return string.Empty;

        return isCorrectAnswer switch
        {
            true => "paper-correct-answer",
            false => "paper-wrong-answer",
            _ => string.Empty
        };
    }

    #endregion
}