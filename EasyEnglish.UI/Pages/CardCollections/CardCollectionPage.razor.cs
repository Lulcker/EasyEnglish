using EasyEnglish.DTO.CardCollections.ResponseModels;
using EasyEnglish.DTO.Cards.RequestModels;
using EasyEnglish.DTO.Cards.ResponseModels;
using EasyEnglish.ProxyApiMethods.ApiMethods;
using EasyEnglish.UI.Contracts;
using EasyEnglish.UI.Extensions;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace EasyEnglish.UI.Pages.CardCollections;

/// <summary>
/// Страница коллекции
/// </summary>
public partial class CardCollectionPage(
    CardApiHelper cardApiHelper,
    CardCollectionApiHelper cardCollectionApiHelper,
    NavigationManager navigationManager,
    IBreadcrumbHelper breadcrumbHelper,
    ISnackbarHelper snackbarHelper
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

    private bool isAddedMode;

    private string newRuWord = string.Empty;
    
    private string newEnWord = string.Empty;

    private bool isDataLoading;

    private bool isUpdateLoading;

    #endregion
    
    #region Properties

    private bool IsSaveButtonDisabled => newRuWord.IsEmpty() || newEnWord.IsEmpty() || isUpdateLoading;

    #endregion

    #region Methods

    protected override async Task OnInitializedAsync()
    {
        isDataLoading = true;
        
        await LoadCardCollectionAsync();
        
        breadcrumbHelper.SetBreadcrumbs(
        [
            new BreadcrumbItem("Коллекции", "/card-collections"),
            new BreadcrumbItem($"{cardCollection?.Title}", $"/card-collection/{CardCollectionId}")
        ]);

        await LoadCardsAsync();
    }

    private async Task LoadCardCollectionAsync() =>
        cardCollection = await cardCollectionApiHelper.GetByIdAsync(CardCollectionId);

    private async Task LoadCardsAsync()
    {
        isDataLoading = true;
        
        cards = [.. await cardApiHelper.AllByCollectionIdAsync(CardCollectionId)];

        isDataLoading = false;
    }
    
    private void SetAddedMode() => isAddedMode = true;

    private void UnsetAndClearAddedMode()
    {
        newRuWord = string.Empty;
        newEnWord = string.Empty;
        isAddedMode = false;
    }

    private async Task SaveAsync()
    {
        isUpdateLoading = true;

        try
        {
            await cardApiHelper.CreateAsync(new CreateCardRequestModel
            {
                RuWord = newRuWord.Trim(),
                EnWord = newEnWord.Trim(),
                CardCollectionId = CardCollectionId
            });
            
            UnsetAndClearAddedMode();

            await snackbarHelper.ShowSuccess("Карточка добавлена");

            await LoadCardsAsync();
        }
        finally
        {
            isUpdateLoading = false;
        }
    }

    private void OpenLevelOnePage() =>
        navigationManager.NavigateTo($"/memorization-level-one/{CardCollectionId}");
    
    private void OpenLevelTwoPage() =>
        navigationManager.NavigateTo($"/memorization-level-two/{CardCollectionId}");
    
    private void OpenLevelThreePage() =>
        navigationManager.NavigateTo($"/memorization-level-three/{CardCollectionId}");
    
    private void OpenTestPage() =>
        navigationManager.NavigateTo($"/test/{CardCollectionId}");

    #endregion
}