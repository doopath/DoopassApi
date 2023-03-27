using System.ComponentModel.DataAnnotations;
using Doopass.Entities;
using Doopass.Exceptions;
using Doopass.Models;

namespace Doopass.Dtos.User;

public class UserDto : IDto<Entities.User>
{
    [MaxLength(255)] public string? Name { get; init; }

    public int? Id { get; init; }

    [MaxLength(255)] [EmailAddress] public string? Email { get; init; }

    public bool IsEmailVerified { get; init; }

    [StringLength(64)] public string? Password { get; init; }

    public int? StoreId { get; set; }

    public List<int>? BackupsIds { get; set; }

    public Entities.User ToEntity()
    {
        var passwordHandler = new PasswordHandler(Password!);

        if (!passwordHandler.IsValid)
            throw new PasswordValidationException(passwordHandler.ValidationMessage!);

        var password = passwordHandler.Hash;

        return new Entities.User
        {
            Name = Name!,
            Id = Id,
            Email = Email!,
            IsEmailVerified = IsEmailVerified,
            Password = password,
            StoreId = StoreId,
            BackupsIds = BackupsIds!
        };
    }
}