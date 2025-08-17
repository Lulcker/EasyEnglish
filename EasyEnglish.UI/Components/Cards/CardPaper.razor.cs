using EasyEnglish.DTO.Cards.RequestModels;
using EasyEnglish.DTO.Cards.ResponseModels;
using EasyEnglish.ProxyApiMethods.ApiMethods;
using EasyEnglish.UI.Contracts;
using EasyEnglish.UI.Extensions;
using Microsoft.AspNetCore.Components;

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
        newRuWord = string.Empty;
        newEnWord = string.Empty;
    }

    private async Task SaveAsync()
    {
        isLoading = true;

        try
        {
            await cardApiHelper.UpdateAsync(new UpdateCardRequestModel
            {
                Id = Card.Id,
                RuWord = newRuWord.Trim(),
                EnWord = newEnWord.Trim()
            });
            
            await snackbarHelper.ShowSuccess("Карточка обновлена");

            Card.RuWord = newRuWord.Trim().UppercaseFirstLetter();
            Card.EnWord = newEnWord.Trim().UppercaseFirstLetter();

            UnsetAndClearUpdateMode();
        }
        finally
        {
            isLoading = false;
        }
    }

    private async Task DeleteAsync()
    {
        if (!await snackbarHelper.ShowConfirm("Вы уверены, что хотите удалить карточку?"))
            return;
        
        await cardApiHelper.DeleteAsync(Card.Id);

        await snackbarHelper.ShowSuccess("Карточка удалена");

        await OnSave.InvokeAsync();
    }

    #endregion
}