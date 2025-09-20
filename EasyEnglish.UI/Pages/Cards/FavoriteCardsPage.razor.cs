using EasyEnglish.DTO.Cards.ResponseModels;
using EasyEnglish.ProxyApiMethods.ApiMethods;
using EasyEnglish.UI.Contracts;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace EasyEnglish.UI.Pages.Cards;

/// <summary>
/// Страница с избранными карточками
/// </summary>
public partial class FavoriteCardsPage(
    CardApiHelper cardApiHelper,
    NavigationManager navigationManager,
    IBreadcrumbHelper breadcrumbHelper
    ) : ComponentBase
{
    #region Fields

    private List<CardResponseModel> cards = [];

    private bool isDataLoading;

    #endregion

    #region Methods

    protected override async Task OnInitializedAsync()
    {
        isDataLoading = true;
        
        breadcrumbHelper.SetBreadcrumbs(
        [
            new BreadcrumbItem("К коллекциям", "/card-collections")
        ]);

        await LoadCardsAsync();
    }

    private async Task LoadCardsAsync()
    {
        isDataLoading = true;
        
        cards = [.. await cardApiHelper.AllFavoriteAsync()];

        isDataLoading = false;
    }

    private void OpenLevelOnePage() =>
        navigationManager.NavigateTo("/memorization-level-one");
    
    private void OpenLevelTwoPage() =>
        navigationManager.NavigateTo("/memorization-level-two");
    
    private void OpenLevelThreePage() =>
        navigationManager.NavigateTo("/memorization-level-three");
    
    private void OpenTestPage() =>
        navigationManager.NavigateTo("/test");

    #endregion
}