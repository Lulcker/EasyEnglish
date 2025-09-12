using EasyEnglish.DTO.Cards.RequestModels;
using EasyEnglish.DTO.Cards.ResponseModels;
using EasyEnglish.DTO.Dictionaries;
using EasyEnglish.ProxyApiMethods.ApiMethods;
using EasyEnglish.UI.Contracts;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace EasyEnglish.UI.Pages.Tests;

public partial class TestPage(
    CardApiHelper cardApiHelper,
    CardCollectionApiHelper cardCollectionApiHelper,
    ISnackbarHelper snackbarHelper,
    IBreadcrumbHelper breadcrumbHelper
    ) : ComponentBase
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

    private bool useAnswerChoice = true;

    private bool useAnswerWriting;
    
    private bool isTestStarted;
    
    private bool isTestFinished;

    private int timeLeft;
    
    private Timer? timer;

    private int countRightAnswer;

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
        
        timer?.Dispose();
        
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
    }
    
    private void Tick(object? state)
    {
        if (timeLeft > 0)
            timeLeft--;
        else
            StopTimer(false);

        InvokeAsync(StateHasChanged);
    }

    private void StopTimer(bool manually)
    {
        isTestStarted = false;
        isTestFinished = true;
            
        timer?.Dispose();

        if (!manually)
            _ = snackbarHelper.ShowWarningMessageBox("Время завершилось!");
    }
    
    private List<string> GetOtherAnswerVariants(Guid currentCardId) =>
    [
        .. cards.Where(c => c.Id != currentCardId)
            .Select(c => c.EnWord)
            .OrderBy(_ => Guid.NewGuid())
            .Take(3)
    ];

    private void OnRightAnswer() => countRightAnswer++;
    
    private static string FormatTime(int seconds) => 
        TimeSpan.FromSeconds(seconds).ToString(@"mm\:ss");
    
    #endregion
}