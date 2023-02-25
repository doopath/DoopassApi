using MediatR;

namespace Doopass.Requests.User;

public class AddNewUserRequest : IRequest<Entities.User>
{
    public required string? Name { get; set; }
    public required string? Email { get; set; }
    public required string? Password { get; set; }
}