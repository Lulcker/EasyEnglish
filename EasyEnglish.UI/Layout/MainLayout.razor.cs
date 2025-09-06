using EasyEnglish.UI.Contracts;
using Microsoft.AspNetCore.Components;

namespace EasyEnglish.UI.Layout;

public partial class MainLayout(
    IBreadcrumbHelper breadcrumbHelper,
    ISnackbarHelper snackbarHelper,
    IUserSession userSession,
    NavigationManager navigationManager
) : IDisposable
{
    #region Methods

    protected override void OnInitialized() =>
        breadcrumbHelper.OnChanged += StateHasChanged;

    private async Task LogOutAsync()
    {
        if (await snackbarHelper.ShowConfirm("Вы уверены, что хотите выйти?"))
            await userSession.EndSession();
    }

    private void OpenProfile() =>
        navigationManager.NavigateTo("/profile");
    
    public void Dispose() =>
        breadcrumbHelper.OnChanged -= StateHasChanged;

    #endregion
}