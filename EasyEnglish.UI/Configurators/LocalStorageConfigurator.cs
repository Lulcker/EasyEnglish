using System.Text.Encodings.Web;
using System.Text.Json;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

namespace EasyEnglish.UI.Configurators;

/// <summary>
/// Конфигурация локального хранилища
/// </summary>
internal static class LocalStorageConfigurator
{
    internal static WebAssemblyHostBuilder ConfigureLocalStorage(this WebAssemblyHostBuilder builder)
    {
        builder.Services.AddBlazoredLocalStorageAsSingleton(cfg =>
        {
            cfg.JsonSerializerOptions = new JsonSerializerOptions
            {
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                WriteIndented = true
            };
        });
        
        return builder;
    }
}