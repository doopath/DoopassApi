using Doopass.Exceptions;
using Doopass.Models;
using Doopass.Repositories;
using Doopass.Requests.User;
using MediatR;

namespace Doopass.RequestHandlers.User;

public class AddNewUserHandler : IRequestHandler<AddNewUserRequest, Entities.User>
{
    private readonly ILogger<AddNewUserHandler> _logger;
    private readonly UsersRepository _repository;
    
    public AddNewUserHandler(ILogger<AddNewUserHandler> logger, UsersRepository repository)
    {
        _logger = logger;
        _repository = repository;
    }
    
    public async Task<Entities.User> Handle(AddNewUserRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Adding a new user with name={Name}", request.Name);
        
        var newUser = ConvertRequestToUser(request);
        
        await _repository.Add(newUser);

        _logger.LogInformation(
            "Added a user with id={Id}, name={Name}", newUser.Id, newUser.Name);
        
        return newUser;
    }

    private Entities.User ConvertRequestToUser(AddNewUserRequest request)
    {
        var passwordHandler = new PasswordHandler(request.Password!);
        
        if (!passwordHandler.IsValid)
            throw new PasswordValidationException(passwordHandler.ValidationMessage!);
        
        var password = passwordHandler.Hash;
        
        return new()
        {
            Name = request.Name,
            Id = null,
            Email = request.Email,
            IsEmailVerified = false,
            Password = password
        };
    }
}