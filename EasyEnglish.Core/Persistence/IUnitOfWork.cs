namespace EasyEnglish.Core.Persistence;

public interface IUnitOfWork
{
    Task SaveChangesAsync();
}