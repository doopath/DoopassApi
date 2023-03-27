using Doopass.Exceptions;
using Doopass.Models;
using Doopass.Repositories;
using Doopass.Requests.User;
using MediatR;

namespace Doopass.RequestHandlers.User;

public class UpdateUserHandler : IRequestHandler<UpdateUserRequest, Entities.User>
{
    private readonly ILogger<UpdateUserHandler> _logger;
    private readonly UsersRepository _repository;

    public UpdateUserHandler(ILogger<UpdateUserHandler> logger, UsersRepository repository)
    {
        _logger = logger;
        _repository = repository;
    }

    public async Task<Entities.User> Handle(UpdateUserRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Updating user with id={Id}", request.Id.ToString());

        var user = ConvertRequestToUser(request);

        EnsurePasswordsMatch(user, request);

        user = await _repository.Update(user);

        _logger.LogInformation("User with id={Id} has been successfully updated!", user.Id.ToString());

        return user;
    }

    private Entities.User ConvertRequestToUser(UpdateUserRequest request)
    {
        return new Entities.User
        {
            Name = request.Name,
            Id = request.Id,
            Email = request.Email,
            IsEmailVerified = request.IsEmailVerified,
            Password = new PasswordHandler(request.Password).Hash,
            StoreId = request.StoreId,
            BackupsIds = request.BackupsIds
        };
    }

    private void EnsurePasswordsMatch(Entities.User user, UpdateUserRequest request)
    {
        if (!PasswordHandler.CompareHash(user.Password!, request.Password))
            throw new PasswordValidationException("Cannot update user data! Wrong password!");
    }
}