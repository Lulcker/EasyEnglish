using System.Text;
using EasyEnglish.Application.Contracts.Providers;
using EasyEnglish.Application.Contracts.Services;
using EasyEnglish.Infrastructure.Providers;
using EasyEnglish.Infrastructure.Services;

namespace EasyEnglish.Configurators;

internal static class HashConfigurator
{
    internal static WebApplicationBuilder ConfigureHash(this WebApplicationBuilder builder)
    {
        var pepper = builder.Configuration.GetValue<string>("Hash:Pepper") ??
                     throw new ArgumentNullException(nameof(builder));
        
        builder.Services.AddSingleton<IHashProvider>(_ => new HashProvider
        {
            PepperBytes = Encoding.UTF8.GetBytes(pepper)
        });

        builder.Services.AddSingleton<IHashService, HashService>();
        
        return builder;
    }
}