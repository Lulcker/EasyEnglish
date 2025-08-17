using EasyEnglish.Core.Persistence;

namespace EasyEnglish.Persistence;

public class UnitOfWork : UnitOfWork<EasyEnglishDbContext>, IUnitOfWork
{
    public UnitOfWork(EasyEnglishDbContext context) : base(context)
    {
    }
}

public interface IUnitOfWork : EasyEnglish.Core.Persistence.IUnitOfWork
{
}