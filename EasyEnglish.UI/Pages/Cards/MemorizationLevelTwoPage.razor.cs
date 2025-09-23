using EasyEnglish.DTO.CardCollections.ResponseModels;
using EasyEnglish.DTO.Cards.ResponseModels;
using EasyEnglish.ProxyApiMethods.ApiMethods;
using EasyEnglish.UI.Contracts;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace EasyEnglish.UI.Pages.Cards;

/// <summary>
/// Уровень 2
/// </summary>
public partial class MemorizationLevelTwoPage(
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
    public Guid? CardCollectionId { get; set; }

    #endregion
    
    #region Fields
    
    private string title = string.Empty;
    
    private CardCollectionResponseModel? cardCollection;

    private List<CardResponseModel> cards = [];

    private CardResponseModel? currentCard;

    private int currentCardIndex;

    private bool isFlipped;

#pragma warning disable CS0414 // Field is assigned but its value is never used
    private bool isLoading;
#pragma warning restore CS0414 // Field is assigned but its value is never used

    #endregion

    #region Methods

    protected override async Task OnInitializedAsync()
    {
        await LoadDataAsync();

        if (CardCollectionId.HasValue)
        {
            breadcrumbHelper.SetBreadcrumbs(
            [
                new BreadcrumbItem("Коллекции", "/card-collections"),
                new BreadcrumbItem($"{cardCollection!.Title}", $"/card-collection/{CardCollectionId}"),
                new BreadcrumbItem("Уровень 2", $"/memorization-level-two/{CardCollectionId}")
            ]);
            
            title = $"{cardCollection!.Title} | Уровень 2";
        }
        else
        {
            breadcrumbHelper.SetBreadcrumbs(
            [
                new BreadcrumbItem("Коллекции", "/card-collections"),
                new BreadcrumbItem("Избранные", "/favorite-cards"),
                new BreadcrumbItem("Уровень 2", "/memorization-level-two")
            ]);
            
            title = "Избранные | Уровень 2";
        }

        currentCard = cards.FirstOrDefault();
    }

    private async Task LoadDataAsync()
    {
        isLoading = true;
        
        if (CardCollectionId.HasValue)
        {
            cardCollection = await cardCollectionApiHelper.GetByIdAsync(CardCollectionId.Value);
        
            cards =  
            [..
                (await cardApiHelper.AllByCollectionIdAsync(CardCollectionId.Value))
                .OrderBy(_ => Guid.NewGuid())
            ];
        }
        else
        {
            cards =
            [..
                (await cardApiHelper.AllFavoriteAsync())
                .OrderBy(_ => Guid.NewGuid())
            ];
        }
        
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