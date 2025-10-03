using EasyEnglish.DTO.Cards.RequestModels;
using EasyEnglish.DTO.Cards.ResponseModels;
using EasyEnglish.ProxyApiMethods;
using EasyEnglish.ProxyApiMethods.ApiMethods;
using EasyEnglish.UI.Contracts;
using EasyEnglish.UI.Extensions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using MudBlazor;

namespace EasyEnglish.UI.Components.Cards;

/// <summary>
/// Карточка карточки
/// </summary>
public partial class CardPaper(
    CardApiHelper cardApiHelper,
    ISnackbarHelper snackbarHelper
    ) : ComponentBase
{
    #region Parameters

    /// <summary>
    /// Коллекция карточек
    /// </summary>
    [Parameter, EditorRequired]
    public required CardResponseModel Card { get; set; }
    
    /// <summary>
    /// Вызывается при нажатии на кнопку Переместить
    /// </summary>
    [Parameter]
    public EventCallback<Guid> OnMove { get; set; }
    
    /// <summary>
    /// Только для чтения
    /// </summary>
    [Parameter]
    public required bool ReadOnly { get; set; }
    
    /// <summary>
    /// Действие сохранения
    /// </summary>
    [Parameter]
    public required EventCallback OnSave { get; set; }

    #endregion

    #region Fields
    
    private string newRuWord = string.Empty;

    private string newEnWord = string.Empty;

    private bool isEditMode;

    private bool isLoading;
    
    private string confirmText = string.Empty;
        
    private bool isConfirmAction;

    #endregion

    #region Refs

    private MudTextField<string> newRuWordTextField = null!;
    
    private MudTextField<string> newEnWordTextField = null!;

    #endregion

    #region Properties

    private bool IsSaveButtonDisabled => (newRuWord.Trim().Equals(Card.RuWord, StringComparison.CurrentCultureIgnoreCase) &&
                                          newEnWord.Trim().Equals(Card.EnWord, StringComparison.CurrentCultureIgnoreCase)) || isLoading;

    #endregion

    #region Methods

    private void SetUpdateMode()
    {
        isEditMode = true;
        newRuWord = Card.RuWord;
        newEnWord = Card.EnWord;
    }

    private void UnsetAndClearUpdateMode()
    {
        isEditMode = false;
        isConfirmAction = false;
        newRuWord = string.Empty;
        newEnWord = string.Empty;
        confirmText = string.Empty;
    }

    private async Task SaveAsync()
    {
        if (IsSaveButtonDisabled)
            return;
        
        isLoading = true;

        try
        {
            await cardApiHelper.UpdateAsync(new UpdateCardRequestModel
            {
                Id = Card.Id,
                RuWord = newRuWord.Trim(),
                EnWord = newEnWord.Trim(),
                IsConfirmAction = isConfirmAction
            });
            
            await snackbarHelper.ShowSuccess("Карточка обновлена");

            Card.RuWord = newRuWord.Trim().UppercaseFirstLetter();
            Card.EnWord = newEnWord.Trim().UppercaseFirstLetter();

            UnsetAndClearUpdateMode();
        }
        catch (ConfirmActionException ex)
        {
            confirmText = ex.Message;
            isConfirmAction = true;
        }
        finally
        {
            isLoading = false;
            await InvokeAsync(StateHasChanged);
        }
    }

    private async Task MoveAsync() =>
        await OnMove.InvokeAsync(Card.Id);
    
    private async Task DeleteAsync()
    {
        if (!await snackbarHelper.ShowConfirm("Вы уверены, что хотите удалить карточку?"))
            return;
        
        await cardApiHelper.DeleteAsync(Card.Id);

        await snackbarHelper.ShowSuccess("Карточка удалена");

        await OnSave.InvokeAsync();
    }

    private async Task ToggleFavoriteAsync()
    {
        Card.IsFavorite = !Card.IsFavorite;
        
        await cardApiHelper.ToggleFavoriteAsync(Card.Id);

        if (Card.IsFavorite)
            await snackbarHelper.ShowSuccess("Карточка добавлена в избранное");
        else
            await snackbarHelper.ShowSuccess("Карточка удалена из избранного");
    }
    
    private async Task HandleKeyDown(KeyboardEventArgs e)
    {
        switch (e.Key)
        {
            case "ArrowUp":
                await newRuWordTextField.FocusAsync();
                break;
            case "ArrowDown":
                await newEnWordTextField.FocusAsync();
                break;
            case "Enter":
                await SaveAsync();
                break;
            case "Escape":
                UnsetAndClearUpdateMode();
                break;
        }
    }

    #endregion
}