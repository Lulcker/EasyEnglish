using EasyEnglish.UI.Contracts;

namespace EasyEnglish.UI.Layout;

public partial class MainLayout(
    IBreadcrumbHelper breadcrumbHelper,
    ISnackbarHelper snackbarHelper,
    IUserSession userSession
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
    
    public void Dispose() =>
        breadcrumbHelper.OnChanged -= StateHasChanged;

    #endregion
}