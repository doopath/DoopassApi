using System.Globalization;
using Doopass.Exceptions;
using Doopass.Models;
using Doopass.Options;
using Doopass.Repositories;
using Doopass.Requests.Store;
using MediatR;
using Microsoft.Extensions.Options;

namespace Doopass.RequestHandlers.Store;

public class AddNewStoreHandler : IRequestHandler<AddNewStoreRequest, Unit>
{
    private readonly ILogger<AddNewStoreHandler> _logger;
    private readonly PathOptions _pathOptions;
    private readonly StoresRepository _storesRepository;
    private readonly UsersRepository _usersRepository;

    public AddNewStoreHandler(
        ILogger<AddNewStoreHandler> logger,
        StoresRepository storesRepository,
        UsersRepository usersRepository,
        IOptions<PathOptions> pathOptions)
    {
        _logger = logger;
        _storesRepository = storesRepository;
        _usersRepository = usersRepository;
        _pathOptions = pathOptions.Value;
    }

    public async Task<Unit> Handle(AddNewStoreRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Adding A new store for the user with id={UserId}", request.UserId.ToString());

        var user = await _usersRepository.GetById(request.UserId);

        EnsurePasswordsMatch(user, request);
        RequireUserHasNoStore(user);

        var store = new Entities.Store
        {
            Id = null,
            UserId = user.Id,
            FilePath = GetStoreFilePath(request.UserId),
            LastUpdateDate = GetUpdateDate()
        };


        await _storesRepository.Add(store);
        await PinStore(store, user);

        _logger.LogInformation(
            "A new store for the user with id={UserId} has been added successfully", request.UserId.ToString());

        return Unit.Value;
    }

    private void EnsurePasswordsMatch(Entities.User user, AddNewStoreRequest request)
    {
        if (!PasswordHandler.CompareHash(user.Password!, request.UserPassword))
            throw new PasswordValidationException(
                $"Cannot add a store fow the user with id={user.Id} - wrong password!");
    }

    private void RequireUserHasNoStore(Entities.User user)
    {
        if (user.Store is not null)
            throw new EntityAlreadyExistsException($"User with id={user.Id} already has a store!");
    }

    private async Task PinStore(Entities.Store store, Entities.User user)
    {
        user.Store = store;
        
        await _usersRepository.Update(user);
    }

    private string GetUpdateDate()
    {
        return DateTime.Now.ToString(CultureInfo.InvariantCulture);
    }

    private string GenerateStoreFileName(int userId)
    {
        var idHash = Hash.GenHash(userId.ToString());

        return $"{idHash}.enc";
    }

    private string GetStoreFilePath(int userId)
    {
        var storeFileName = GenerateStoreFileName(userId);

        return _pathOptions.StoreStoragePath + storeFileName;
    }
}