using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace EasyEnglish.Persistence;

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<EasyEnglishDbContext>
{
    public EasyEnglishDbContext CreateDbContext(string[] args)
    {
        var builder = new DbContextOptionsBuilder<EasyEnglishDbContext>();

        builder.UseNpgsql("", _ => { });

        return new EasyEnglishDbContext(builder.Options);
    }
}