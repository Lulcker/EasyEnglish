using EasyEnglish.Application.Contracts.Providers;
using EasyEnglish.Application.Contracts.Services;
using EasyEnglish.Infrastructure.Providers;
using EasyEnglish.Infrastructure.Services;

namespace EasyEnglish.Configurators;

internal static class EmailServiceConfigurator
{
    internal static WebApplicationBuilder ConfigureEmailService(this WebApplicationBuilder builder)
    {
        var host = builder.Configuration.GetValue<string>("Email:Host") ??
                     throw new ArgumentNullException(nameof(builder));
        
        var port = builder.Configuration.GetValue<int?>("Email:Port") ??
                     throw new ArgumentNullException(nameof(builder));
        
        var emailTo = builder.Configuration.GetValue<string>("Email:EmailTo") ??
                     throw new ArgumentNullException(nameof(builder));
        
        var password = builder.Configuration.GetValue<string>("Email:Password") ??
                     throw new ArgumentNullException(nameof(builder));
        
        builder.Services.AddSingleton<IEmailProvider>(_ => new EmailProvider
        {
            Host = host,
            Port = port,
            EmailTo = emailTo,
            Password = password
        });
        
        builder.Services.AddScoped<IEmailService, EmailService>();
        
        return builder;
    }
}