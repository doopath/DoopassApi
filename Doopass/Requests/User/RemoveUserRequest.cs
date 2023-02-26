using MediatR;

namespace Doopass.Requests.User;

public class RemoveUserRequest : IRequest<Unit>
{
    public required int Id { get; set; }
    public required string Password { get; set; }
}