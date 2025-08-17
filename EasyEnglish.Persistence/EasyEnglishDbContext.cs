using EasyEnglish.Persistence.Configs;
using Microsoft.EntityFrameworkCore;

namespace EasyEnglish.Persistence;

public class EasyEnglishDbContext : DbContext
{
    public EasyEnglishDbContext(DbContextOptions<EasyEnglishDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(CardConfig).Assembly);
    }
}