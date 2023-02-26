using Doopass.Entities;

namespace Doopass.Dtos;

public interface IDto<T> where T : IEntity
{
    /// <summary>
    ///     Convert the data transfer object to the entity.
    /// </summary>
    /// <returns>Instance of the entity.</returns>
    public T ToEntity();
}