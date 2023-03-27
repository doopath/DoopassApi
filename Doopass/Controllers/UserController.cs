using Doopass.Dtos.User;
using Doopass.Exceptions;
using Doopass.Requests.User;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Doopass.Controllers;

public class UserController : BaseController
{
    private readonly ILogger<UserController> _logger;
    private readonly IMediator _mediator;

    public UserController(ILogger<UserController> logger, IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }

    private ActionResult HandleException(Exception exc)
    {
        _logger.LogWarning(exc.Message);

        if (exc is EntityWasNotFoundException)
            return new NotFoundObjectResult(exc.Message);

        return new ConflictObjectResult(exc.Message);
    }

    #region Actions

    [HttpPost]
    public async Task<ActionResult<UserDto>> AddNewUser(AddNewUserRequest request)
    {
        try
        {
            var user = await _mediator.Send(request);

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
            var user = await _mediator.Send(request);

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
}