using Doopass.Entities;
using Doopass.Options;
using Microsoft.Extensions.Options;

namespace Doopass.Repositories;

public abstract class EntityRepository<T> where T : IEntity
{
    protected readonly DbOptions Options;

    public EntityRepository(IOptions<DbOptions> options)
    {
        Options = options.Value;
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