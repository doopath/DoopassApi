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
    private readonly StoresRepository _storesRepository;
    private readonly UsersRepository _usersRepository;
    private readonly PathOptions _pathOptions;

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
        _logger.LogInformation("Adding a new store for the user with id={Id}", request.UserId);

        await AddNewUser(request);
        
        
        _logger.LogInformation("Added a new store for the user with id={Id}", request.UserId);
        
        return Unit.Value;
    }

    private async Task AddNewUser(AddNewStoreRequest request)
    {
        var storeFilePath = GetStoreFilePath(request.UserId);
        var updateDate = GetUpdateDate();
        var store = new Entities.Store()
        {
            Id = null,
            UserId = request.UserId,
            User = null,
            FilePath = storeFilePath,
            LastUpdateDate = updateDate,
        };


        await EnsureUserExists(request);
        await _storesRepository.Add(store);
        await PinStore(store, request);
    }

    private async Task EnsureUserExists(AddNewStoreRequest request)
    {
        if (!await _usersRepository.DoesEntityExist(request.UserId))
            throw new EntityWasNotFoundException($"User with id={request.UserId} was not found!");
    }

    private async Task PinStore(Entities.Store store, AddNewStoreRequest request)
    {
        await _usersRepository.Update(new Entities.User
                {
                    Id = request.UserId,
                    Name = null,
                    Email = null,
                    IsEmailVerified = false,
                    Password = new PasswordHandler(request.UserPassword).Hash,
                    Store = store,
                    StoreId = store.Id,
                    BackupsIds = null
                });
    }

    private string GetUpdateDate()
        => DateTime.Now.ToString(CultureInfo.InvariantCulture);

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