using Microsoft.AspNetCore.Mvc;
using Doopass.Repositories;
using Doopass.Dtos.UserDto;
using Doopass.Entities;
using Doopass.Exceptions;
using Doopass.Models;
using Doopass.Options;
using Doopass.Requests.User;
using MediatR;
using Microsoft.Extensions.Options;

namespace Doopass.Controllers;

public class UserController : BaseController
{
    private readonly UsersRepository _repository;
    private readonly ILogger<UserController> _logger;
    private readonly IMediator _mediator;

    public UserController(IOptions<DbOptions> dbOptions, ILogger<UserController> logger, IMediator mediator)
    {
        _repository = new UsersRepository(dbOptions);
        _logger = logger;
        _mediator = mediator;
    }
    
    #region Actions
    [HttpPost]
    public async Task<ActionResult<UserDto>> AddNewUser(AddNewUserRequest request)
    {
        try
        {
            User user = await _mediator.Send(request);
            return new ActionResult<UserDto>(user.ToDto());
        }
        catch (EmailAlreadyExistsException exc)
        {
            _logger.LogWarning(exc.Message);
            return new ConflictObjectResult(exc.Message);
        }
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
    public async Task<ActionResult<NewUserDto>> UpdateUser(UpdateUserDto updateUserDto)
    {
        _logger.LogInformation("Updating user with id={Id}", updateUserDto.Id!.Value);
        
        try
        {
            var targetUser = await _repository.GetById(updateUserDto.Id.Value);
            var user = updateUserDto.ToEntity();
            
            EnsurePasswordsMatch(user.Password!, targetUser.Password!);
            
            user = await _repository.Update(user);
            
            return new ActionResult<NewUserDto>(user.ToNewDto());
        }
        catch (Exception exc) when (exc is EntityWasNotFoundException
                                        or EmailAlreadyExistsException
                                        or PasswordValidationException)
        {
            _logger.LogWarning(exc.Message);
            return new ConflictObjectResult(exc.Message);
        }
    }

    [HttpPost]
    public async Task<ActionResult> UpdateUserPassword(UpdateUserPasswordDto updateUserPasswordDto)
    {
        _logger.LogWarning("Updating password of a user with id={Id}", updateUserPasswordDto.Id);
       
        try
        {
            var updates = updateUserPasswordDto.ToEntity();
            var updatingUser = await _repository.GetById(updates.Id!.Value);
            var oldPasswordHash = new PasswordHandler(updateUserPasswordDto.OldPassword).Hash;

            EnsurePasswordsMatch(updatingUser.Password!, oldPasswordHash);
            await _repository.Update(updates);

            _logger.LogInformation(
                "Password of the user with id={Id} has been successfully updated!", updates.Id);
            
            return new OkResult();
        }
        catch (Exception exc) when (exc is EntityWasNotFoundException
                                        or EmailAlreadyExistsException
                                        or PasswordValidationException)
        {
            _logger.LogWarning(exc.Message);
            
            if (exc is EntityWasNotFoundException)
                return new NotFoundObjectResult(exc.Message);
            
            return new ConflictObjectResult(exc.Message);
        }
    }
    #endregion 
    
    private void EnsurePasswordsMatch(string sample, string target)
    {
        if (!sample.Equals(target))
            throw new PasswordValidationException("Cannot update user data! Wrong password!");
    }
}