using EasyEnglish.Application.Contracts.Providers;
using EasyEnglish.Infrastructure.Providers;

namespace EasyEnglish.Configurators;

/// <summary>
/// Конфигурация CORS
/// </summary>
internal static class CorsConfigurator
{
    internal static WebApplicationBuilder ConfigureCors(this WebApplicationBuilder builder)
    {
        
#if DEBUG
        const string webUrl = "https://localhost:7216";
#else
            var webUrl = builder.Configuration.GetValue<string>("WebBaseUrl") 
                         ?? throw new ArgumentNullException(nameof(builder));
#endif
        
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("CorsPolicy", policy => 
                policy.WithOrigins(webUrl)
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials());
        });

        builder.Services.AddSingleton<IUrlUIProvider>(new UrlUIProvider
        {
            Url = webUrl
        });

        return builder;
    }
}