using EasyEnglish.Persistence;
using Microsoft.EntityFrameworkCore;

namespace EasyEnglish.Configurators;
internal static class DatabaseConfigurator
{
    internal static WebApplicationBuilder ConfigureDatabase(this WebApplicationBuilder builder)
    {
        builder.Services.AddDbContext<EasyEnglishDbContext>(options =>
            options.UseNpgsql(builder.Configuration.GetConnectionString("Postgres")));
        
        return builder;
    }

    internal static void MigrateDatabase(this WebApplication builder)
    {
        using var dbContext = builder.Services.CreateScope().ServiceProvider.GetRequiredService<EasyEnglishDbContext>();
        dbContext.Database.Migrate();
    }
}