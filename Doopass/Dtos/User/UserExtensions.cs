using Doopass.Entities;

namespace Doopass.Dtos.User;

public static class UserExtensions
{
    public static UserDto ToDto(this Entities.User user)
    {
        return new()
        {
            Name = user.Name,
            Id = user.Id!.Value,
            Email = user.Email,
            IsEmailVerified = user.IsEmailVerified,
            Password = user.Password,
            BackupsIds = user.BackupsIds,
            Store = user.Store
        };
    }
}