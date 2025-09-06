using Hangfire;
using Hangfire.PostgreSql;

namespace EasyEnglish.Configurators;

internal static class HangfireConfigurator
{
    internal static WebApplicationBuilder ConfigureHangfire(this WebApplicationBuilder builder)
    {
        builder.Services.AddHangfire(cfg =>
        {
            cfg.UseSimpleAssemblyNameTypeSerializer();
            cfg.UseRecommendedSerializerSettings();
            cfg.UsePostgreSqlStorage(pgOptions =>
            {
                pgOptions.UseNpgsqlConnection(builder.Configuration.GetConnectionString("Postgres"));
            });
        });
        
        builder.Services.AddHangfireServer();
        
        return builder;
    }
}