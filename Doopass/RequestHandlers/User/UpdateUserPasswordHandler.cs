using Doopass.Exceptions;
using Doopass.Models;
using Doopass.Repositories;
using Doopass.Requests.User;
using MediatR;

namespace Doopass.RequestHandlers.User;

public class UpdateUserPasswordHandler : IRequestHandler<UpdateUserPasswordRequest, Unit>
{
    private readonly ILogger<UpdateUserPasswordHandler> _logger;
    private readonly UsersRepository _repository;

    public UpdateUserPasswordHandler(ILogger<UpdateUserPasswordHandler> logger, UsersRepository repository)
    {
        _logger = logger;
        _repository = repository;
    }

    public async Task<Unit> Handle(UpdateUserPasswordRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Updating password of a user with id={Id}", request.Id);

        var updates = ConvertRequestToUser(request);
        var updatingUser = await _repository.GetById(updates.Id!.Value);

        EnsurePasswordsMatch(updatingUser, request);
        await _repository.Update(updates);

        _logger.LogInformation(
            "Password of the user with id={Id} has been successfully updated!", updates.Id);

        return Unit.Value;
    }

    private Entities.User ConvertRequestToUser(UpdateUserPasswordRequest request)
    {
        var passwordHandler = new PasswordHandler(request.NewPassword);

        if (!passwordHandler.IsValid)
            throw new PasswordValidationException(passwordHandler.ValidationMessage!);

        var password = passwordHandler.Hash;

        return new Entities.User
        {
            Name = null,
            Id = request.Id,
            Email = null,
            // It's ok to stay this as true. If the existing user's
            // email is not verified this will still be false.
            IsEmailVerified = true,
            Password = password,
            StoreId = null,
            BackupsIds = null
        };
    }

    private void EnsurePasswordsMatch(Entities.User existingUser, UpdateUserPasswordRequest request)
    {
        if (!PasswordHandler.CompareHash(existingUser.Password!, request.OldPassword))
            throw new PasswordValidationException("Cannot update user data! Wrong password!");
    }
}