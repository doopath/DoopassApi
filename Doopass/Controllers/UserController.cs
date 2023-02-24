using Microsoft.AspNetCore.Mvc;
using Doopass.Repositories;
using Doopass.Dtos.UserDto;
using Doopass.Exceptions;
using Doopass.Options;
using Microsoft.Extensions.Options;

namespace Doopass.Controllers;

public class UserController : BaseController
{
    private readonly UsersRepository _repository;


    public UserController(IOptions<DbOptions> dbSettings)
    {
        _repository = new UsersRepository(dbSettings.Value);
    }
    
    [HttpPost]
    public async Task<ActionResult<NewUserDto>> AddNewUser(NewUserDto newUserDto)
    {
        newUserDto.IsEmailVerified = false;
        var user = newUserDto.ToEntity();
        
        try
        {
            await _repository.Add(user);
        }
        catch (EmailAlreadyExistsException exc)
        {
            return new ConflictObjectResult(exc.Message);
        }
        
        return new ActionResult<NewUserDto>(user.ToNewDto());
    }
    
    [HttpPost]
    public async Task<ActionResult<string>> RemoveUser([FromForm] int id = 0)
    {
        try
        {
            var user = await _repository.GetById(id);
            await _repository.Remove(user);
            return new ActionResult<string>($"User with id={id} was successfully removed!");
        }
        catch (EntityWasNotFoundException exc)
        {
            return new NotFoundObjectResult(exc.Message);
        }
    }

    [HttpPost]
    public async Task<ActionResult<NewUserDto>> UpdateUser(UpdateUserDto newUserDto)
    {
        try
        {
            var user = await _repository.Update(newUserDto.ToEntity());
            return new ActionResult<NewUserDto>(user.ToNewDto());
        }
        catch (EntityWasNotFoundException exc)
        {
            return new NotFoundObjectResult(exc.Message);
        }
        catch (EmailAlreadyExistsException exc)
        {
            return new NotFoundObjectResult(exc.Message);
        }
    }
}