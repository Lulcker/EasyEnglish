using EasyEnglish.DTO.CardCollections.RequestModels;
using EasyEnglish.DTO.CardCollections.ResponseModels;
using EasyEnglish.ProxyApiMethods.ApiMethods;
using EasyEnglish.UI.Contracts;
using EasyEnglish.UI.Extensions;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace EasyEnglish.UI.Pages.CardCollections;

/// <summary>
/// Страница коллекций карточек
/// </summary>
public partial class CardCollectionsPage(
    CardCollectionApiHelper cardCollectionApiHelper,
    ISnackbarHelper snackbarHelper
    ) : ComponentBase
{
    #region Fields

    private List<CardCollectionResponseModel> cardCollections = [];

    private bool isAddedMode;

    private string newCollectionTitle = string.Empty;

    private bool isDataLoading;

    private bool isUpdateLoading;

    #endregion

    #region Properties

    private bool IsSaveButtonDisabled => newCollectionTitle.IsEmpty() || isUpdateLoading;

    #endregion
    
    #region Methods

    protected override async Task OnInitializedAsync() =>
        await LoadDataAsync();

    private async Task LoadDataAsync()
    {
        isDataLoading = true;

        cardCollections = [.. await cardCollectionApiHelper.AllAsync()];
        
        isDataLoading = false;
    }

    private void SetAddedMode() => isAddedMode = true;

    private void UnsetAndClearAddedMode()
    {
        newCollectionTitle = string.Empty;
        isAddedMode = false;
    }

    private async Task Save()
    {
        isUpdateLoading = true;

        try
        {
            await cardCollectionApiHelper.CreateAsync(new CreateCardCollectionRequestModel
            {
                Title = newCollectionTitle.Trim()
            });
            
            UnsetAndClearAddedMode();

            await snackbarHelper.ShowSuccess("Коллекция добавлена");

            await LoadDataAsync();
        }
        finally
        {
            isUpdateLoading = false;
        }
    }

    #endregion
}