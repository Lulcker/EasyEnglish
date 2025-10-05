using EasyEnglish.Application.Contracts.Providers;
using EasyEnglish.Application.Contracts.Services;
using EasyEnglish.Infrastructure.Providers;
using EasyEnglish.Infrastructure.Services;

namespace EasyEnglish.Configurators;

internal static class EmailServiceConfigurator
{
    internal static WebApplicationBuilder ConfigureEmailService(this WebApplicationBuilder builder)
    {
        var host = builder.Configuration.GetValue<string>("Email:Host");
        
        ArgumentException.ThrowIfNullOrWhiteSpace(host);
        
        var port = builder.Configuration.GetValue<int?>("Email:Port");
        
        ArgumentNullException.ThrowIfNull(port);
        
        var emailTo = builder.Configuration.GetValue<string>("Email:EmailTo");
        
        ArgumentException.ThrowIfNullOrWhiteSpace(emailTo);
        
        var password = builder.Configuration.GetValue<string>("Email:Password");
        
        ArgumentException.ThrowIfNullOrWhiteSpace(password);
        
        builder.Services.AddSingleton<IEmailProvider>(_ => new EmailProvider
        {
            Host = host,
            Port = port.Value,
            EmailTo = emailTo,
            Password = password
        });
        
        builder.Services.AddScoped<IEmailService, EmailService>();
        
        return builder;
    }
}