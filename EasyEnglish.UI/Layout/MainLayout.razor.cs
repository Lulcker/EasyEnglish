using EasyEnglish.UI.Contracts;

namespace EasyEnglish.UI.Layout;

public partial class MainLayout(
    ISnackbarHelper snackbarHelper,
    IUserSession userSession
)
{
    #region Methods

    private async Task LogOutAsync()
    {
        if (await snackbarHelper.ShowConfirm("Вы уверены, что хотите выйти?"))
            await userSession.EndSession();
    }

    #endregion
}