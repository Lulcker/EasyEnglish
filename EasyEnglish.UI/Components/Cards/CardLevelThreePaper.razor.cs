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
    /// Ответ
    /// </summary>
    [Parameter, EditorRequired]
    public required string Answer { get; set; }
    
    /// <summary>
    /// Событие изменения ответа
    /// </summary>
    [Parameter, EditorRequired]
    public required EventCallback<string> AnswerChanged { get; set; }
    
    /// <summary>
    /// Правильный ответ?
    /// </summary>
    [Parameter, EditorRequired]
    public required bool? IsCorrectAnswer { get; set; }
    
    /// <summary>
    /// Событие изменения правильности ответа
    /// </summary>
    [Parameter, EditorRequired]
    public required EventCallback<bool?> IsCorrectAnswerChanged { get; set; }
    
    /// <summary>
    /// Событие при неправильном ответе
    /// </summary>
    [Parameter, EditorRequired]
    public EventCallback OnWrongAnswer { get; set; }

    #endregion

    #region Methods

    private async Task UpdateAnswer(string answer) =>
        await AnswerChanged.InvokeAsync(answer);

    private async Task CheckAnswer()
    {
        var resultEquals = string.Equals(Answer.Trim(), Card.EnWord, StringComparison.CurrentCultureIgnoreCase);
        
        await IsCorrectAnswerChanged.InvokeAsync(resultEquals);
        
        if (!resultEquals)
            await OnWrongAnswer.InvokeAsync();
    }
    
    private string GetTextFieldClass()
    {
        if (!IsCorrectAnswer.HasValue)
            return string.Empty;

        return IsCorrectAnswer switch
        {
            true => "paper-correct-answer",
            false => "paper-wrong-answer",
            _ => string.Empty
        };
    }

    #endregion
}