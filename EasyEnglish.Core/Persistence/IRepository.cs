using System.Collections;

namespace EasyEnglish.Core.Persistence;

public interface IRepository<TEntity> :
    IQueryable<TEntity>,
    IEnumerable<TEntity>,
    IEnumerable,
    IQueryable,
    IAsyncEnumerable<TEntity>
    where TEntity : class
{
    void Add(TEntity objectToAdd);

    void AddRange(IEnumerable<TEntity> objectsToAdd);

    void Remove(TEntity objectToRemove);

    void RemoveRange(IEnumerable<TEntity> objectsToRemove);
}