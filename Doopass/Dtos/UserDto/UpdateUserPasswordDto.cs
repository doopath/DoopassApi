using Doopass.Entities;
using Doopass.Exceptions;
using Doopass.Models;

namespace Doopass.Dtos.UserDto;

public class UpdateUserPasswordDto : IDto<User>
{
    public required string OldPassword { get; set; }
    public required string NewPassword { get; set; }
    public required int Id { get; set; }

    public User ToEntity()
    {
        var passwordHandler = new PasswordHandler(NewPassword);
        
        if (!passwordHandler.IsValid)
            throw new PasswordValidationException(passwordHandler.ValidationMessage!);
        
        string password = passwordHandler.Hash;
        
        return new()
        {
            Id = Id,
            Name = null,
            Email = null,
            Password = password,
            IsEmailVerified = false,
        };
    }
}