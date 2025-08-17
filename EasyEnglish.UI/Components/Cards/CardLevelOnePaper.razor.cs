using EasyEnglish.DTO.Cards.ResponseModels;
using EasyEnglish.UI.Extensions;
using Microsoft.AspNetCore.Components;

namespace EasyEnglish.UI.Components.Cards;

/// <summary>
/// Карточка для уровня 1
/// </summary>
public partial class CardLevelOnePaper : ComponentBase
{
    #region Parameters

    /// <summary>
    /// Карточка
    /// </summary>
    [Parameter, EditorRequired]
    public required CardResponseModel Card { get; set; }

    /// <summary>
    /// Другие варианты ответов
    /// </summary>
    [Parameter]
    public List<string> OtherAnswerVariants { get; set; } = [];

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

    #endregion

    #region Fields

    private List<string> answerVariants = [];

    private string answer = string.Empty;

    private bool isCorrectAnswer;

    #endregion

    #region Properties

    private string GetPaperClass(string variant)
    {
        if (answer.IsEmpty())
            return "paper-not-answer";
        
        if (Card.EnWord == variant)
            return "paper-correct-answer";

        return isCorrectAnswer switch
        {
            true when answer == variant => "paper-correct-answer",
            false when answer == variant => "paper-wrong-answer",
            _ => string.Empty
        };
    }

    #endregion

    #region Methods

    protected override void OnInitialized()
    {
        answerVariants = 
        [..
            OtherAnswerVariants
                .Concat([Card.EnWord])
                .OrderBy(_ => Guid.NewGuid())
        ];
    }

    private async Task SetAnswer(string currentAnswer)
    {
        if (answer.IsNotEmpty())
            return;
        
        answer = currentAnswer;

        if (answer == Card.EnWord)
        {
            isCorrectAnswer = true;
        }
        else
        {
            isCorrectAnswer = false;
            await OnWrongAnswer.InvokeAsync();
        }
    }

    #endregion
}