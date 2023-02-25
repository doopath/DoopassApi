using Doopass.Entities;
using MediatR;

namespace Doopass.Requests.User;

public class UpdateUserRequest : IRequest<Entities.User>
{
    public string? Name { get; set; }
    public required int? Id { get; set; }
    public string? Email { get; set; }
    public bool IsEmailVerified { get; set; }
    public required string Password { get; set; }
}