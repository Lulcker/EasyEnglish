using EasyEnglish.UI.Contracts;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

namespace EasyEnglish.UI.Configurators;

/// <summary>
/// Конфигурация авторизации
/// </summary>
internal static class AuthorizationConfigurator
{
    internal static WebAssemblyHostBuilder ConfigureAuthorization(this WebAssemblyHostBuilder builder)
    {
        builder.Services.AddSingleton<IUserSession, UserSession>();
        
        builder.Services.AddSingleton<AuthenticationStateProvider>(s => (AuthenticationStateProvider)s.GetRequiredService<IUserSession>());

        builder.Services.AddAuthorizationCore();
        
        return builder;
    }
}