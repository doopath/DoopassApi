using MediatR;

namespace Doopass.Requests.User;

public class UpdateUserPasswordRequest : IRequest<Unit>
{
    public required string OldPassword { get; set; }
    public required string NewPassword { get; set; }
    public required int Id { get; set; }
}