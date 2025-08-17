using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

namespace EasyEnglish.UI.Configurators;

/// <summary>
/// Конфигурация HttpClient
/// </summary>
internal static class HttpClientConfigurator
{
    internal static WebAssemblyHostBuilder ConfigureHttpClient(this WebAssemblyHostBuilder builder)
    {
#if DEBUG
        const string apiUrl = "https://localhost:7046/";
#else
        const string apiUrl = "http://localhost:8081/";
#endif
        
        builder.Services.AddScoped(_ => new HttpClient { BaseAddress = new Uri(apiUrl) });
        
        return builder;
    }
}