using EasyEnglish.DTO.CardCollections.ResponseModels;
using EasyEnglish.DTO.Cards.ResponseModels;
using EasyEnglish.ProxyApiMethods.ApiMethods;
using EasyEnglish.UI.Components.Cards;
using EasyEnglish.UI.Contracts;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace EasyEnglish.UI.Pages.Cards;

/// <summary>
/// Уровень 3
/// </summary>
public partial class MemorizationLevelThreePage(
    CardApiHelper cardApiHelper,
    CardCollectionApiHelper cardCollectionApiHelper,
    IBreadcrumbHelper breadcrumbHelper
) : ComponentBase
{
    #region Parameters

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
    
    private readonly HashSet<CardResponseModel> wrongAnswerCards = [];
    
    private bool isShowWrongCards;

    private bool givenAnswer;

#pragma warning disable CS0414 // Field is assigned but its value is never used
    private bool isLoading;
#pragma warning restore CS0414 // Field is assigned but its value is never used

    #endregion

    #region Refs

    private CardLevelThreePaper cardLevelThreePaper = null!;

    #endregion

    #region Methods

    protected override async Task OnInitializedAsync()
    {
        await LoadDataAsync();
        
        breadcrumbHelper.SetBreadcrumbs(
        [
            new BreadcrumbItem("Коллекции", "/card-collections"),
            new BreadcrumbItem($"{cardCollection?.Title}", $"/card-collection/{CardCollectionId}"),
            new BreadcrumbItem("Уровень 3", $"/memorization-level-three/{CardCollectionId}")
        ]);

        currentCard = cards.FirstOrDefault();
    }

    private async Task LoadDataAsync()
    {
        isLoading = true;
        
        cardCollection = await cardCollectionApiHelper.GetByIdAsync(CardCollectionId);
        
        cards =  
        [..
            (await cardApiHelper.AllByCollectionIdAsync(CardCollectionId))
                .OrderBy(_ => Guid.NewGuid())
        ];
        
        isLoading = false;
    }

    private void NextCard()
    {
        cardLevelThreePaper.ClearAnswer();
        givenAnswer = false;
        currentCard = cards[++currentCardIndex];
    }

    private void SetGivenAnswer() =>
        givenAnswer = true;
    
    private void AddWrongCard(CardResponseModel card)
        => wrongAnswerCards.Add(card);

    private void ToggleShowWrongCards() =>
        isShowWrongCards = !isShowWrongCards;

    #endregion
}