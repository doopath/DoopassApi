using System.Globalization;
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
        
        await _storesRepository.Add(new Entities.Store()
        {
            Id = null,
            UserId = request.UserId,
            User = null,
            FilePath = storeFilePath,
            LastUpdateDate = updateDate,
        });
    }

    private async Task PinStore(AddNewStoreRequest request)
    {
        
    }

    private string GetUpdateDate()
        => DateTime.Now.ToString(CultureInfo.InvariantCulture);

    private string GenerateStoreFileName(int userId)
    {
        var idHash = new Hash(userId.ToString()).ToString();
        
        return $"{idHash}.enc";
    }
    
    private string GetStoreFilePath(int userId)
    {
        var storeFileName = GenerateStoreFileName(userId);

        return _pathOptions.StoreStoragePath + storeFileName;
    }
}