using System.ComponentModel.DataAnnotations;
using Doopass.Entities;
using Doopass.Exceptions;
using Doopass.Models;

namespace Doopass.Dtos.UserDto;

public class UserDto : IDto<User>
{
    [MaxLength(255)] public string? Name { get; init; }

    public int? Id { get; init; }

    [MaxLength(255)] [EmailAddress] public string? Email { get; init; }

    public bool IsEmailVerified { get; init; }

    [StringLength(64)] public string? Password { get; init; }

    public Store? Store { get; set; }

    public List<int>? BackupsIds { get; set; }

    public User ToEntity()
    {
        var passwordHandler = new PasswordHandler(Password!);

        if (!passwordHandler.IsValid)
            throw new PasswordValidationException(passwordHandler.ValidationMessage!);

        var password = passwordHandler.Hash;

        return new User
        {
            Name = Name!,
            Id = Id,
            Email = Email!,
            IsEmailVerified = IsEmailVerified,
            Password = password,
            Store = Store,
            BackupsIds = BackupsIds!
        };
    }
}