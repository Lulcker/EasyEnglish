using EasyEnglish.DTO.CardCollections.ResponseModels;
using EasyEnglish.DTO.Cards.ResponseModels;
using EasyEnglish.ProxyApiMethods.ApiMethods;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace EasyEnglish.UI.Pages.Cards;

/// <summary>
/// Уровень 2
/// </summary>
public partial class MemorizationLevelTwoPage(
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

    private bool isFlipped;

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
            new BreadcrumbItem("Уровень 2", $"/memorization-level-two/{CardCollectionId}")
        ];

        currentCard = cards.FirstOrDefault();
    }

    private async Task LoadDataAsync()
    {
        isLoading = true;
        
        cardCollection = await cardCollectionApiHelper.GetByIdAsync(CardCollectionId);
        
        cards =  
        [..
            (await cardApiHelper.AllByCollectionIdAsync(CardCollectionId))
            .OrderBy(x => Guid.NewGuid())
        ];
        
        isLoading = false;
    }

    private async Task PreviousCard()
    {
        isFlipped = false;
        
        await Task.Delay(250);
        
        currentCard = cards[--currentCardIndex];
    }

    private async Task NextCard()
    {
        isFlipped = false;
        
        await Task.Delay(250);
        
        currentCard = cards[++currentCardIndex];
    }

    #endregion
}