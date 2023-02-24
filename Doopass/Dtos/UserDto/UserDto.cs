using Doopass.Entities;
using Doopass.Exceptions;
using Doopass.Models;

namespace Doopass.Dtos.UserDto;

public abstract class UserDto : IDto<User>
{
    public virtual string? Name { get; set; }
    public virtual int? Id { get; set; }
    public virtual string? Email { get; set; }
    public virtual bool IsEmailVerified { get; set; }
    public virtual string? Password { get; set; }
    
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