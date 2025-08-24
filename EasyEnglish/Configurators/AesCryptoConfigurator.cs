using System.Text;
using EasyEnglish.Application.Contracts.Providers;
using EasyEnglish.Application.Contracts.Services;
using EasyEnglish.Infrastructure.Providers;
using EasyEnglish.Infrastructure.Services;

namespace EasyEnglish.Configurators;

internal static class AesCryptoConfigurator
{
    internal static WebApplicationBuilder ConfigureAesCrypto(this WebApplicationBuilder builder)
    {
        var key = builder.Configuration.GetValue<string>("Aes:Key") ??
                     throw new ArgumentNullException("Aes:Key");
        
        var iv = builder.Configuration.GetValue<string>("Aes:IV") ??
                 throw new ArgumentNullException("Aes:IV");
        
        builder.Services.AddSingleton<IAesCryptoProvider>(_ => new AesCryptoProvider
        {
            Key = Encoding.UTF8.GetBytes(key),
            IV = Encoding.UTF8.GetBytes(iv)
        });

        builder.Services.AddSingleton<IAesCryptoService, AesCryptoService>();
        
        return builder;
    }
}