using Hangfire;
using Hangfire.PostgreSql;

namespace EasyEnglish.Configurators;

internal static class HangfireConfigurator
{
    internal static WebApplicationBuilder ConfigureHangfire(this WebApplicationBuilder builder)
    {
        var postgresConnection = builder.Configuration.GetConnectionString("Postgres");
        
        ArgumentException.ThrowIfNullOrWhiteSpace(postgresConnection);
        
        builder.Services.AddHangfire(cfg =>
        {
            cfg.UseSimpleAssemblyNameTypeSerializer();
            cfg.UseRecommendedSerializerSettings();
            cfg.UsePostgreSqlStorage(pgOptions =>
            {
                pgOptions.UseNpgsqlConnection(postgresConnection);
            });
        });
        
        builder.Services.AddHangfireServer();
        
        return builder;
    }
}