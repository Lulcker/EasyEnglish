using EasyEnglish.Core.Domain;
using EasyEnglish.Core.Persistence;

namespace EasyEnglish.Persistence;

public class Repository<TEntity> : Repository<EasyEnglishDbContext, TEntity>, IRepository<TEntity> where TEntity : EntityBase
{
    public Repository(EasyEnglishDbContext context) : base(context)
    {
    }
}

public interface IRepository<TEntity> : EasyEnglish.Core.Persistence.IRepository<TEntity> where TEntity : class
{
}