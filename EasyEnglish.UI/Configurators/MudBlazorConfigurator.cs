using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor;
using MudBlazor.Services;

namespace EasyEnglish.UI.Configurators;

/// <summary>
/// Конфигурация Mud Blazor
/// </summary>
internal static class MudBlazorConfigurator
{
    internal static WebAssemblyHostBuilder ConfigureMudBlazor(this WebAssemblyHostBuilder builder)
    {
        builder.Services.AddMudServices(cfg =>
        {
            cfg.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.BottomRight;
            cfg.SnackbarConfiguration.PreventDuplicates = false;
            cfg.SnackbarConfiguration.NewestOnTop = false;
            cfg.SnackbarConfiguration.ShowCloseIcon = true;
            cfg.SnackbarConfiguration.VisibleStateDuration = 5000;
            cfg.SnackbarConfiguration.HideTransitionDuration = 500;
            cfg.SnackbarConfiguration.ShowTransitionDuration = 500;
            cfg.SnackbarConfiguration.SnackbarVariant = Variant.Filled;
        });

        builder.Services.AddSingleton<IDialogService, DialogService>();
        builder.Services.AddSingleton<ISnackbar, SnackbarService>();
        
        return builder;
    }
}