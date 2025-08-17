namespace EasyEnglish.Configurators;

/// <summary>
/// Конфигурация CORS
/// </summary>
internal static class CorsConfigurator
{
    internal static WebApplicationBuilder ConfigureCors(this WebApplicationBuilder builder)
    {
        builder.Services.AddCors(options =>
        {
#if DEBUG
            const string webUrl = "https://localhost:7216";
#else
            var webUrl = builder.Configuration.GetValue<string>("WebBaseUrl") 
                         ?? throw new ArgumentNullException("webUrl");
#endif
            
            options.AddPolicy("CorsPolicy", policy => 
                policy.WithOrigins(webUrl)
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials());
        });

        return builder;
    }
}