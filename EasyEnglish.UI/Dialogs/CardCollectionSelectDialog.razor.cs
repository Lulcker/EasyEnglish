using EasyEnglish.DTO.CardCollections.ResponseModels;
using EasyEnglish.DTO.Cards.RequestModels;
using EasyEnglish.ProxyApiMethods.ApiMethods;
using EasyEnglish.UI.Contracts;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace EasyEnglish.UI.Dialogs;

/// <summary>
/// Диалог с выбором коллекции (пока только для перемещения карточки)
/// </summary>
public partial class CardCollectionSelectDialog(
    CardApiHelper cardApiHelper,
    CardCollectionApiHelper cardCollectionApiHelper,
    ISnackbarHelper snackbarHelper
    ) : ComponentBase
{
    #region Parameters
    
    /// <summary>
    /// Действие после сохранения
    /// </summary>
    [Parameter, EditorRequired]
    public required EventCallback AfterSave { get; set; }

    #endregion

    #region Readonly

    private static readonly DialogOptions options = new()
    {
        MaxWidth = MaxWidth.ExtraSmall,
        FullWidth = true
    };

    #endregion

    #region Fields

    private bool isDataLoading;
    
    private Guid currentCardId;

    private Guid currentCardCollectionId;

    private CardCollectionResponseModel? selectedCardCollection;

    private bool isOpened;

    private List<CardCollectionResponseModel> cardCollections = [];

    #endregion

    #region Properties

    private bool IsMoveButtonDisabled => isDataLoading || selectedCardCollection is null;

    #endregion

    #region Methods

    public async Task OpenAsync(Guid cardId, Guid cardCollectionId)
    {
        selectedCardCollection = null!;
        
        currentCardId = cardId;
        currentCardCollectionId = cardCollectionId;

        isOpened = true;
        StateHasChanged();

        await LoadDataAsync();
    }

    private async Task CloseAsync()
    {
        await AfterSave.InvokeAsync();
        
        isOpened = false;
        StateHasChanged();
    }

    private async Task LoadDataAsync()
    {
        isDataLoading = true;
        
        cardCollections = [.. await cardCollectionApiHelper.AllAsync()];

        isDataLoading = false;
    }

    private async Task MoveAsync()
    {
        isDataLoading = true;

        try
        {
            await cardApiHelper.MoveAsync(new MoveCardRequestModel
            {
                CardId = currentCardId,
                CardCollectionId = selectedCardCollection!.Id
            });

            await CloseAsync();

            await snackbarHelper.ShowSuccess($"Карточка перенесена в коллекцию {selectedCardCollection.Title}");
        }
        finally
        {
            isDataLoading = false;
            StateHasChanged();
        }
    }

    #endregion
}