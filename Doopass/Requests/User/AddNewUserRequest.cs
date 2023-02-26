using System.ComponentModel.DataAnnotations;
using MediatR;

namespace Doopass.Requests.User;

public class AddNewUserRequest : IRequest<Entities.User>
{
    [MaxLength(255)] public required string? Name { get; set; }

    [MaxLength(255)] [EmailAddress] public required string? Email { get; set; }

    public required string? Password { get; set; }
}