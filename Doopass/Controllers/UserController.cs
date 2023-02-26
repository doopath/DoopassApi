using Microsoft.AspNetCore.Mvc;
using Doopass.Repositories;
using Doopass.Dtos.UserDto;
using Doopass.Entities;
using Doopass.Exceptions;
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
            return HandleException(exc);
        }
    }
    
    [HttpPost]
    public async Task<ActionResult<string>> RemoveUser(RemoveUserRequest request)
    {
        try
        {
            await _mediator.Send(request);
            
            return new ActionResult<string>($"User with id={request.Id} has been successfully removed");
        }
        catch (EntityWasNotFoundException exc)
        {
            return HandleException(exc);
        }
    }

    [HttpPost]
    public async Task<ActionResult<UserDto>> UpdateUser(UpdateUserRequest request)
    {
        try
        {
            User user = await _mediator.Send(request);
            
            return new ActionResult<UserDto>(user.ToDto());
        }
        catch (Exception exc) when (exc is EntityWasNotFoundException
                                        or EmailAlreadyExistsException
                                        or PasswordValidationException)
        {
            return HandleException(exc);
        }
    }

    [HttpPost]
    public async Task<ActionResult> UpdateUserPassword(UpdateUserPasswordRequest request)
    {
        try
        {
            await _mediator.Send(request);
            return new OkResult();
        }
        catch (Exception exc) when (exc is EntityWasNotFoundException
                                        or EmailAlreadyExistsException
                                        or PasswordValidationException)
        {
            return HandleException(exc);
        }
    }
    #endregion 
    
    private ActionResult HandleException(Exception exc)
    {
        _logger.LogWarning(exc.Message);
        
        if (exc is EntityWasNotFoundException)
            return new NotFoundObjectResult(exc.Message);
        
        return new ConflictObjectResult(exc.Message);
    }
}