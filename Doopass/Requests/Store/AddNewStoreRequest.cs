using MediatR;

namespace Doopass.Requests.Store;

public class AddNewStoreRequest : IRequest<Unit>
{
    public required int UserId { get; init; }
    public required string StoreContent { get; init; }
    public required string UserPassword { get; init; }
}