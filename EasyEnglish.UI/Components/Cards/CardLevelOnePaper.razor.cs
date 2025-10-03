using EasyEnglish.DTO.Cards.ResponseModels;
using EasyEnglish.ProxyApiMethods.ApiMethods;
using EasyEnglish.UI.Contracts;
using EasyEnglish.UI.Extensions;
using Microsoft.AspNetCore.Components;

namespace EasyEnglish.UI.Components.Cards;

/// <summary>
/// Карточка для уровня 1
/// </summary>
public partial class CardLevelOnePaper(
    CardApiHelper cardApiHelper,
    ISnackbarHelper snackbarHelper
    ) : ComponentBase
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
    [Parameter, EditorRequired]
    public List<string> OtherAnswerVariants { get; set; } = [];

    /// <summary>
    /// Текущий индекс карточки
    /// </summary>
    [Parameter, EditorRequired] 
    public int Index { get; set; }
    
    /// <summary>
    /// Количество карточек
    /// </summary>
    [Parameter, EditorRequired]
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

    private List<string> answerVariants = [];

    private string answer = string.Empty;

    private bool isCorrectAnswer;

    #endregion

    #region Properties

    private string GetPaperClass(string variant)
    {
        if (ReadOnly && answer.IsEmpty())
            return string.Empty;
        
        if (answer.IsEmpty())
            return "paper-not-answer";

        if (!ShowAnswer)
        {
            return answer == variant 
                ? "paper-not-show-answer" 
                : string.Empty;
        }
        
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
            await OnRightAnswer.InvokeAsync();
        }
        else
        {
            isCorrectAnswer = false;
            await OnWrongAnswer.InvokeAsync();
        }
    }
    
    private async Task ToggleFavoriteAsync()
    {
        Card.IsFavorite = !Card.IsFavorite;
        
        await cardApiHelper.ToggleFavoriteAsync(Card.Id);

        if (Card.IsFavorite)
            await snackbarHelper.ShowSuccess("Карточка добавлена в избранное");
        else
            await snackbarHelper.ShowSuccess("Карточка удалена из избранного");
    }

    #endregion
}