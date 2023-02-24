using Doopass.Entities;

namespace Doopass.Dtos.UserDto;

public abstract class UserDto : IDto<User>
{
    public virtual string? Name { get; set; }
    public virtual int? Id { get; set; }
    public virtual string? Email { get; set; }
    public virtual bool IsEmailVerified { get; set; }
    
    public virtual User ToEntity()
    {
        return new()
        {
            Name = Name,
            Id = Id,
            Email = Email,
            IsEmailVerified = IsEmailVerified
        };
    }
}