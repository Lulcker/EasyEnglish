using EasyEnglish.DTO.Cards.RequestModels;
using EasyEnglish.DTO.Cards.ResponseModels;
using EasyEnglish.DTO.Dictionaries;
using EasyEnglish.ProxyApiMethods.ApiMethods;
using EasyEnglish.UI.Contracts;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace EasyEnglish.UI.Pages.Cards;

/// <summary>
/// Страница с тестом
/// </summary>
public partial class TestPage(
    CardApiHelper cardApiHelper,
    CardCollectionApiHelper cardCollectionApiHelper,
    ISnackbarHelper snackbarHelper,
    IBreadcrumbHelper breadcrumbHelper
    ) : ComponentBase, IAsyncDisposable
{
    #region Parameters

    /// <summary>
    /// Id коллекции карточек
    /// </summary>
    [Parameter]
    public Guid? CardCollectionId { get; set; }

    #endregion
    
    #region Fields
    
    private List<CardForTestResponseModel> cards = [];

    private bool useAnswerChoice;

    private bool useAnswerWriting = true;
    
    private bool isTestStarted;
    
    private bool isTestFinished;

    private int timeLeft;
    
    private Timer? timer;

    private int countRightAnswer;

    private int countWrongAnswer;

    private bool isDataLoading;
    
    #endregion
    
    #region Properties
    
    private bool IsStartButtonDisabled => !useAnswerChoice && !useAnswerWriting;
    
    #endregion

    #region Methods

    protected override async Task OnInitializedAsync()
    {
        if (CardCollectionId.HasValue)
        {
            var cardCollection = await cardCollectionApiHelper.GetByIdAsync(CardCollectionId.Value);
            
            breadcrumbHelper.SetBreadcrumbs(
            [
                new BreadcrumbItem("Коллекции", "/card-collections"),
                new BreadcrumbItem($"{cardCollection?.Title}", $"/card-collection/{CardCollectionId}"),
                new BreadcrumbItem("Тест", $"/test/{CardCollectionId}")
            ]);
        }
        else
        {
            breadcrumbHelper.SetBreadcrumbs([new BreadcrumbItem("К коллекциям", "/")]);
        }
    }

    private async Task LoadDataAsync()
    {
        isDataLoading = true;

        await InvokeAsync(StateHasChanged);
        
        cards = [.. await cardApiHelper.GetForTestAsync(new CardForTestRequestModel
        {
            CardCollectionId = CardCollectionId,
            UseAnswerChoice = useAnswerChoice,
            UseAnswerWriting = useAnswerWriting
        })];

        isDataLoading = false;
    }

    private async Task StartTestAsync()
    {
        if (!await snackbarHelper.ShowConfirm("Вы уверены, что хотите начать тест?"))
            return;
        
        isTestStarted = true;
        isTestFinished = false;
        
        await LoadDataAsync();
        
        timeLeft = cards.Sum(x => x.Level is CardLevel.One ? 5 : 10);
        timer = new Timer(Tick, null, 1000, 1000);
    }

    private async Task StopTestAsync()
    {
        if (!await snackbarHelper.ShowConfirm("Вы уверены, что хотите завершить тест?"))
            return;
        
        if (isTestFinished)
            return;
        
        StopTimer(true);
    }
    
    private void ResetTest()
    {
        isTestStarted = false;
        isTestFinished = false;

        cards = [];

        countRightAnswer = 0;
        countWrongAnswer = 0;
    }
    
    private void Tick(object? state)
    {
        if (timeLeft > 0)
            timeLeft--;
        else
            StopTimer(false, "Время завершилось!");

        InvokeAsync(StateHasChanged);
    }

    private void StopTimer(bool manually, string message = "")
    {
        isTestStarted = false;
        isTestFinished = true;
        
        timer?.Dispose();
        timer = null;
        timeLeft = 0;

        if (!manually)
            _ = snackbarHelper.ShowWarningMessageBox(message);
    }
    
    private List<string> GetOtherAnswerVariants(Guid currentCardId) =>
    [
        .. cards.Where(c => c.Id != currentCardId)
            .Select(c => c.EnWord)
            .OrderBy(_ => Guid.NewGuid())
            .Take(3)
    ];

    private void OnRightAnswer()
    {
        countRightAnswer++;
        
        CheckAndStopTimer();
    }
    
    private void OnWrongAnswer()
    {
        countWrongAnswer++;

        CheckAndStopTimer();
    }

    private void CheckAndStopTimer()
    {
        if (countRightAnswer + countWrongAnswer == cards.Count)
            StopTimer(true);
    }
    
    private static string FormatTime(int seconds) => 
        TimeSpan.FromSeconds(seconds).ToString(@"mm\:ss");
    
    public async ValueTask DisposeAsync()
    {
        if (timer is not null)
            await timer.DisposeAsync();
        
        GC.SuppressFinalize(this);
    }
    
    #endregion
}
