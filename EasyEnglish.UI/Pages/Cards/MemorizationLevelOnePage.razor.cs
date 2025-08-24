using EasyEnglish.DTO.CardCollections.ResponseModels;
using EasyEnglish.DTO.Cards.ResponseModels;
using EasyEnglish.ProxyApiMethods.ApiMethods;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace EasyEnglish.UI.Pages.Cards;

/// <summary>
/// Уровень 1
/// </summary>
public partial class MemorizationLevelOnePage(
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

    private List<CardResponseModel> wrongAnswerCards = [];
    
    private bool isShowWrongCards;

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
            new BreadcrumbItem($"{cardCollection?.Title}", $"/card-collection/{CardCollectionId}"),
            new BreadcrumbItem("Уровень 1", $"/memorization-level-one/{CardCollectionId}")
        ];
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

    private List<string> GetOtherAnswerVariants(Guid currentCardId) =>
    [
        .. cards.Where(c => c.Id != currentCardId)
            .Select(c => c.EnWord)
            .OrderBy(_ => Guid.NewGuid())
            .Take(3)
    ];

    private void AddWrongCard(CardResponseModel card)
        => wrongAnswerCards.Add(card);

    private void ToggleShowWrongCards() =>
        isShowWrongCards = !isShowWrongCards;

    #endregion
}