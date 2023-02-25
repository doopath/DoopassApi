using Doopass.Entities;

namespace Doopass.Dtos.UserDto;

public static class UserExtensions
{
    public static NewUserDto ToNewDto(this User user) 
        => new()
        {
            Name = user.Name,
            Id = user.Id,
            Email = user.Email,
            IsEmailVerified = user.IsEmailVerified,
            Password = user.Password
        };
    
    public static UpdateUserDto ToUpdateDto(this User user)
        => new()
        {
            Name = user.Name,
            Id = user.Id!.Value,
            Email = user.Email,
            IsEmailVerified = user.IsEmailVerified,
            Password = user.Password!
        };
    
    public static UserDto ToDto(this User user)
        => new()
        {
            Name = user.Name,
            Id = user.Id!.Value,
            Email = user.Email,
            IsEmailVerified = user.IsEmailVerified,
            Password = user.Password
        };

    public static void UpdateOf(this User user, UpdateUserDto from)
    {
        user.Name = from.Name ?? user.Name;
        user.Email = from.Email ?? user.Email;
        user.Password = from.Password ?? user.Password;
        
        if (user.Email != from.Email)
            user.IsEmailVerified = false;
        else
            user.IsEmailVerified &= from.IsEmailVerified;
    }
}