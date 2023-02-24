using Microsoft.AspNetCore.Mvc;
using Doopass.Repositories;
using Doopass.Dtos.UserDto;
using Doopass.Entities;
using Doopass.Exceptions;
using Doopass.Options;
using Microsoft.Extensions.Options;

namespace Doopass.Controllers;

public class UserController : BaseController
{
    private readonly UsersRepository _repository;
    private readonly ILogger<UserController> _logger;

    public UserController(IOptions<DbOptions> dbSettings, ILogger<UserController> logger)
    {
        _repository = new UsersRepository(dbSettings.Value);
        _logger = logger;
    }
    
    [HttpPost]
    public async Task<ActionResult<NewUserDto>> AddNewUser(NewUserDto newUserDto)
    {
        _logger.LogInformation("Adding new user with id={Id}", newUserDto.Id!.Value);
        
        newUserDto.IsEmailVerified = false;
        var user = newUserDto.ToEntity();
        
        try
        {
            await _repository.Add(user);
        }
        catch (EmailAlreadyExistsException exc)
        {
            _logger.LogWarning(exc.Message);
            return new ConflictObjectResult(exc.Message);
        }
        
        return new ActionResult<NewUserDto>(user.ToNewDto());
    }
    
    [HttpPost]
    public async Task<ActionResult<string>> RemoveUser([FromForm] int id = 0)
    {
        _logger.LogInformation("Removing user with id={Id}", id);
        
        try
        {
            var user = await _repository.GetById(id);
            await _repository.Remove(user);
            return new ActionResult<string>($"User with id={id} was successfully removed!");
        }
        catch (EntityWasNotFoundException exc)
        {
            _logger.LogWarning(exc.Message);
            return new NotFoundObjectResult(exc.Message);
        }
    }

    [HttpPost]
    public async Task<ActionResult<NewUserDto>> UpdateUser(UpdateUserDto newUserDto)
    {
        _logger.LogInformation("Updating user with id={Id}", newUserDto.Id!.Value);
        
        try
        {
            var targetUser = await _repository.GetById(newUserDto.Id.Value);
            var user = newUserDto.ToEntity();
            
            EnsurePasswordsMatch(user, targetUser);
            
            user = await _repository.Update(user);
            
            return new ActionResult<NewUserDto>(user.ToNewDto());
        }
        catch (Exception exc) when (exc is EntityWasNotFoundException
                                        or EmailAlreadyExistsException
                                        or PasswordValidationException)
        {
            _logger.LogWarning(exc.Message);
            return new NotFoundObjectResult(exc.Message);
        }
    }

    private void EnsurePasswordsMatch(User user, User target)
    {
        if (user.Password != target.Password)
            throw new PasswordValidationException("Cannot update user data! Wrong password!");
    }
}