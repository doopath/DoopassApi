using Doopass.Requests.Store;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Doopass.Controllers;

public class StoreController : BaseController
{
    private readonly ILogger<StoreController> _logger;
    private readonly IMediator _mediator;

    public StoreController(ILogger<StoreController> logger, IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<ActionResult> AddNewStore(AddNewStoreRequest request)
    {
        try
        {
            await _mediator.Send(request);
            return new OkObjectResult($"A new store for the user with id={request.UserId} has been added successfully");
        }
        catch (Exception exc)
        {
            _logger.LogWarning(exc.Message);
            return new ConflictObjectResult(exc.Message);
        }
    }
}