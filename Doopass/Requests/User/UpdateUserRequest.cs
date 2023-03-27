using System.ComponentModel.DataAnnotations;
using MediatR;

namespace Doopass.Requests.User;

public class UpdateUserRequest : IRequest<Entities.User>
{
    [MaxLength(255)] public string? Name { get; set; }

    public required int Id { get; set; }

    [MaxLength(255)] [EmailAddress] public string? Email { get; set; }

    public bool IsEmailVerified { get; set; }

    public required string Password { get; set; }

    public int? StoreId { get; set; }

    public List<int>? BackupsIds { get; set; }
}