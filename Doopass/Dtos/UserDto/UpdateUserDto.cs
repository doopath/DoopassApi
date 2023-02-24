using Doopass.Entities;

namespace Doopass.Dtos.UserDto;

public class UpdateUserDto : IDto<User>
{
    public string? Name { get; set; }
    public required int Id { get; set; } = default;
    public string? Email { get; set; }
    public bool IsEmailVerified { get; set; } = false;
    
    public User ToEntity()
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