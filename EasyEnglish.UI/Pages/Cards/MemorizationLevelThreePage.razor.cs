using EasyEnglish.DTO.CardCollections.ResponseModels;
using EasyEnglish.DTO.Cards.ResponseModels;
using EasyEnglish.ProxyApiMethods.ApiMethods;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace EasyEnglish.UI.Pages.Cards;

/// <summary>
/// Уровень 3
/// </summary>
public partial class MemorizationLevelThreePage(
    CardApiHelper cardApiHelper,
    CardCollectionApiHelper cardCollectionApiHelper
) : ComponentBase
{
    #region Properties

    /// <summary>
    /// Id коллекции карточек
    /// </summary>
    [Parameter]
    public required Guid CardCollectionId { get; set; }

    #endregion
    
    #region Fields
    
    private CardCollectionResponseModel? cardCollection;

    private List<CardResponseModel> cards = [];

    private CardResponseModel? currentCard;

    private int currentCardIndex;

    private string answer = string.Empty;

    private bool? correctAnswer;

#pragma warning disable CS0414 // Field is assigned but its value is never used
    private bool isLoading;
#pragma warning restore CS0414 // Field is assigned but its value is never used

    #endregion
    
    #region Breadcrumbs

    private List<BreadcrumbItem> breadcrumbItems = [];

    #endregion

    #region Methods

    protected override async Task OnInitializedAsync()
    {
        await LoadDataAsync();
        
        breadcrumbItems =
        [
            new BreadcrumbItem("Коллекции", "/card-collections"),
            new BreadcrumbItem($"Коллекция {cardCollection?.Title}", $"/card-collection/{CardCollectionId}"),
            new BreadcrumbItem("Уровень 3", $"/memorization-level-three/{CardCollectionId}")
        ];

        currentCard = cards.FirstOrDefault();
    }

    private async Task LoadDataAsync()
    {
        isLoading = true;
        
        cardCollection = await cardCollectionApiHelper.GetByIdAsync(CardCollectionId);
        
        cards =  [.. await cardApiHelper.AllByCollectionIdAsync(CardCollectionId)];
        
        isLoading = false;
    }

    private void PreviousCard()
    {
        answer = string.Empty;
        correctAnswer = null;
        
        currentCard = cards[--currentCardIndex];
    }

    private void NextCard()
    {
        answer = string.Empty;
        correctAnswer = null;
        
        currentCard = cards[++currentCardIndex];
    }

    #endregion
}