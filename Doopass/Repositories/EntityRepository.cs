using Doopass.Entities;
using Doopass.Options;

namespace Doopass.Repositories;

public abstract class EntityRepository<T> where T : IEntity
{
    protected readonly DbOptions Options;
    
    public EntityRepository(DbOptions options)
    {
        Options = options;
    }
    
    public virtual async Task Add(T entity)
    {
        await using var context = new DoopassContext(Options);
        await context.AddAsync(entity);
        await context.SaveChangesAsync();
    }

    public virtual async Task Remove(T entity)
    {
        await using var context = new DoopassContext(Options);
        context.Remove(entity);
        await context.SaveChangesAsync();
    }
}