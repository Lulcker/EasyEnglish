using EasyEnglish.DTO.CardCollections.RequestModels;
using EasyEnglish.DTO.CardCollections.ResponseModels;
using EasyEnglish.ProxyApiMethods.ApiMethods;
using EasyEnglish.UI.Contracts;
using EasyEnglish.UI.Extensions;
using Microsoft.AspNetCore.Components;

namespace EasyEnglish.UI.Components.CardCollections;

public partial class CardCollectionPaper(
    CardCollectionApiHelper cardCollectionApiHelper,
    NavigationManager navigationManager,
    ISnackbarHelper snackbarHelper
    ) : ComponentBase
{
    #region Parameters

    /// <summary>
    /// Коллекция карточек
    /// </summary>
    [Parameter, EditorRequired]
    public required CardCollectionResponseModel CardCollection { get; set; }
    
    /// <summary>
    /// Действие сохранения
    /// </summary>
    [Parameter, EditorRequired]
    public required EventCallback OnSave { get; set; }

    #endregion

    #region Fields
    
    private string newTitle = string.Empty;

    private bool isEditMode;

    private bool isLoading;

    #endregion

    #region Properties

    private bool IsSaveButtonDisabled => newTitle.Trim().Equals(CardCollection.Title, StringComparison.CurrentCultureIgnoreCase) || isLoading;

    #endregion

    #region Methods

    private void SetUpdateMode()
    {
        isEditMode = true;
        newTitle = CardCollection.Title;
    }

    private void UnsetAndClearUpdateMode()
    {
        isEditMode = false;
        newTitle = string.Empty;
    }

    private async Task SaveAsync()
    {
        isLoading = true;

        try
        {
            await cardCollectionApiHelper.UpdateAsync(new UpdateCardCollectionRequestModel
            {
                Id = CardCollection.Id,
                Title = newTitle.Trim()
            });
            
            await snackbarHelper.ShowSuccess("Коллекция обновлена");

            CardCollection.Title = newTitle.Trim().UppercaseFirstLetter();

            UnsetAndClearUpdateMode();
        }
        finally
        {
            isLoading = false;
        }
    }

    private async Task DeleteAsync()
    {
        if (!await snackbarHelper.ShowConfirm("Вы уверены, что хотите удалить коллекцию?"))
            return;
        
        await cardCollectionApiHelper.DeleteAsync(CardCollection.Id);

        await snackbarHelper.ShowSuccess("Коллекция удалена");

        await OnSave.InvokeAsync();
    }

    private void OpenCardCollectionPage() =>
        navigationManager.NavigateTo($"/card-collection/{CardCollection.Id}");

    #endregion
}