using Microsoft.EntityFrameworkCore;

namespace EasyEnglish.Core.Persistence;

public class UnitOfWork<TDbContext>(TDbContext context) : IUnitOfWork where TDbContext : DbContext
{
    public async Task SaveChangesAsync() =>
        await context.SaveChangesAsync();
}