using Doopass.Exceptions;
using Doopass.Models;
using Doopass.Repositories;
using Doopass.Requests.User;
using MediatR;

namespace Doopass.RequestHandlers.User;

public class RemoveUserHandler : IRequestHandler<RemoveUserRequest, Unit>
{
    private readonly ILogger<RemoveUserHandler> _logger;
    private readonly UsersRepository _repository;

    public RemoveUserHandler(ILogger<RemoveUserHandler> logger, UsersRepository repository)
    {
        _logger = logger;
        _repository = repository;
    }

    public async Task<Unit> Handle(RemoveUserRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Removing user with id={Id}", request.Id);

        var user = await _repository.GetById(request.Id);
        EnsurePasswordsMatch(user, request);
        await _repository.Remove(user);

        _logger.LogInformation("User with id={Id} has been successfully removed", request.Id);

        return Unit.Value;
    }

    private void EnsurePasswordsMatch(Entities.User user, RemoveUserRequest request)
    {
        if (!PasswordHandler.CompareHash(user.Password, request.Password))
            throw new PasswordValidationException("Cannot update user data! Wrong password!");
    }
}