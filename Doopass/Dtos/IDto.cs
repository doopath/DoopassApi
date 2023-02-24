using Doopass.Entities;

namespace Doopass.Dtos;

public interface IDto<T> where T : IEntity
{
    public T ToEntity();
}