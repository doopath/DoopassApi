using Doopass.Dtos.UserDto;
using Doopass.Entities;
using Doopass.Exceptions;
using Doopass.Options;
using Microsoft.Extensions.Options;

namespace Doopass.Repositories;

public class UsersRepository : EntityRepository<User>
{
    public UsersRepository(IOptions<DbOptions> options) : base(options)
    { }

    #region Database Actions
    public async Task<User> GetById(int id)
    {
        await using var context = new DoopassContext(Options);
        User? user = await context.FindAsync<User>(id);
        
        return user ?? throw new EntityWasNotFoundException($"User with id={id} was not found!");
    }

    public override async Task Add(User user)
    {
        await using var context = new DoopassContext(Options);

        if (DoesEmailExist(context, user.Email!))
        {
            await context.DisposeAsync();
            throw new EmailAlreadyExistsException($"User with email={user.Email} already exists!");
        }

        await context.AddAsync(user);
        await context.SaveChangesAsync();
    }
    
    
    public async Task<User> Update(User user)
    {
        await using var context = new DoopassContext(Options);
        
        User? userToUpdate = await context.FindAsync<User>(user.Id);
        
        if (userToUpdate is null) 
            throw new EntityWasNotFoundException($"User with id={user.Id} was not found!");

        if (user.Email != userToUpdate.Email && DoesEmailExist(context, user.Email!))
            throw new EmailAlreadyExistsException($"User with email={user.Email} already exists!");

        userToUpdate.UpdateOf(user);
        
        await context.SaveChangesAsync();
        
        return userToUpdate;
    }
    #endregion

    private bool DoesEmailExist(DoopassContext? context, string email)
    {
        return context?.Users?.AsParallel().Any(user => user.Email == email) ?? false;
    }
}