using EasyEnglish.UI.Contracts;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;

namespace EasyEnglish.UI.Layout;

public partial class MainLayout(
    IBreadcrumbHelper breadcrumbHelper,
    ISnackbarHelper snackbarHelper,
    IUserSession userSession,
    NavigationManager navigationManager
    ) : IDisposable
{
    #region Fields

    private string currentPage = string.Empty;

    #endregion
    
    #region Methods

    protected override void OnInitialized() =>
        currentPage = navigationManager.ToBaseRelativePath(navigationManager.Uri);

    protected override void OnAfterRender(bool firstRender)
    {
        if (!firstRender)
            return;
        
        breadcrumbHelper.OnChanged += StateHasChanged;
        navigationManager.LocationChanged += OnLocationChanged;
    }

    private async Task LogOutAsync()
    {
        if (await snackbarHelper.ShowConfirm("Вы уверены, что хотите выйти?"))
            await userSession.EndSession();
    }

    private void OpenProfile() =>
        navigationManager.NavigateTo("/profile");

    private void OpenTest() =>
        navigationManager.NavigateTo("/test");
    
    private void OnLocationChanged(object? _, LocationChangedEventArgs e)
    {
        currentPage = navigationManager.ToBaseRelativePath(e.Location);
        StateHasChanged();
    }
    
    public void Dispose()
    {
        breadcrumbHelper.OnChanged -= StateHasChanged;
        navigationManager.LocationChanged -= OnLocationChanged;
        
        GC.SuppressFinalize(this);
    }

    #endregion
}