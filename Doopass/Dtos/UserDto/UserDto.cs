using Doopass.Entities;
using Doopass.Exceptions;
using Doopass.Models;

namespace Doopass.Dtos.UserDto;

public class UserDto : IDto<User>
{
    public string? Name { get; init; }
    public int? Id { get; init; }
    public string? Email { get; init; }
    public bool IsEmailVerified { get; init; }
    public string? Password { get; init; }
    
    public User ToEntity()
    {
        var passwordHandler = new PasswordHandler(Password!);
        
        if (!passwordHandler.IsValid)
            throw new PasswordValidationException(passwordHandler.ValidationMessage!);
        
        string password = passwordHandler.Hash;
        
        return new()
        {
            Name = Name,
            Id = Id,
            Email = Email,
            IsEmailVerified = IsEmailVerified,
            Password = password
        };
    }
}