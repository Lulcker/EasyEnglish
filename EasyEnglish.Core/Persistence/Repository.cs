using System.Collections;
using System.Linq.Expressions;
using EasyEnglish.Core.Domain;
using Microsoft.EntityFrameworkCore;

namespace EasyEnglish.Core.Persistence;

public class Repository<TDbContext, TEntity> : 
    IRepository<TEntity>
    where TEntity : EntityBase
    where TDbContext : DbContext
{
    private readonly TDbContext context;
    private readonly IQueryable<TEntity> sourceQuery;
    
    public Repository(TDbContext context)
    {
        this.context = context;
        sourceQuery = context.Set<TEntity>();
    }
    
    public void Add(TEntity objectToAdd) =>
        context.Set<TEntity>().Add(objectToAdd);

    public void AddRange(IEnumerable<TEntity> objectsToAdd) =>
        context.Set<TEntity>().AddRange(objectsToAdd);

    public void Remove(TEntity objectToRemove) =>
        context.Set<TEntity>().Remove(objectToRemove);

    public void RemoveRange(IEnumerable<TEntity> objectsToRemove) =>
        context.Set<TEntity>().RemoveRange(objectsToRemove);

    public IEnumerator<TEntity> GetEnumerator() => sourceQuery.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public Type ElementType => sourceQuery.ElementType;

    public Expression Expression => sourceQuery.Expression;

    public IQueryProvider Provider => sourceQuery.Provider;
    
    public IAsyncEnumerator<TEntity> GetAsyncEnumerator(CancellationToken cancellationToken = default) =>
        ((IAsyncEnumerable<TEntity>)sourceQuery).GetAsyncEnumerator(cancellationToken);
}