using EasyEnglish.UI.Contracts;
using MudBlazor;

namespace EasyEnglish.UI.Helpers;

public class SnackbarHelper(
    ISnackbar snackbar,
    IDialogService dialogService
    ) : ISnackbarHelper
{
    public ValueTask ShowSuccess(string message)
    {
        snackbar.Add(message, Severity.Success);
        return ValueTask.CompletedTask;
    }

    public ValueTask ShowInfo(string message)
    {
        snackbar.Add(message, Severity.Info);
        return ValueTask.CompletedTask;
    }

    public ValueTask ShowError(string message)
    {
        snackbar.Add(message, Severity.Error, cfg =>
        {
            cfg.CloseAfterNavigation = true;
        });
        return ValueTask.CompletedTask;
    }

    public async ValueTask<bool> ShowConfirm(
        string message,
        string header = "Предупреждение",
        string confirmButtonText = "Да",
        string cancelButtonText = "Нет")
    {
        var result = await dialogService.ShowMessageBox(
            title: header,
            message: message,
            yesText: confirmButtonText,
            noText: cancelButtonText
        );

        return result.HasValue && result.Value;
    }

    public async ValueTask ShowWarningMessageBox(string message, string header = "Внимаение")
    {
        var result = await dialogService.ShowMessageBox(
            title: header,
            message: message
        );
    }
}