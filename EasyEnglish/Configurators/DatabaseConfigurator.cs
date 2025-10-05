using EasyEnglish.Persistence;
using Microsoft.EntityFrameworkCore;

namespace EasyEnglish.Configurators;

internal static class DatabaseConfigurator
{
    internal static WebApplicationBuilder ConfigureDatabase(this WebApplicationBuilder builder)
    {
        var postgresConnection = builder.Configuration.GetConnectionString("Postgres");
        
        ArgumentException.ThrowIfNullOrWhiteSpace(postgresConnection);
        
        builder.Services.AddDbContext<EasyEnglishDbContext>(options =>
            options.UseNpgsql(postgresConnection));
        
        return builder;
    }

    internal static void MigrateDatabase(this WebApplication builder)
    {
        using var dbContext = builder.Services.CreateScope().ServiceProvider.GetRequiredService<EasyEnglishDbContext>();
        dbContext.Database.Migrate();
    }
}