using Doopass.Entities;

namespace Doopass.Dtos.UserDto;

public static class UserExtensions
{
    public static UserDto ToDto(this User user)
        => new()
        {
            Name = user.Name,
            Id = user.Id!.Value,
            Email = user.Email,
            IsEmailVerified = user.IsEmailVerified,
            Password = user.Password
        };
}