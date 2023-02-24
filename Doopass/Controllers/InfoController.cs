using Doopass.Models;
using Doopass.Options;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Doopass.Controllers;

public class InfoController : BaseController
{
    private readonly InfoOptions _options;

    public InfoController(IOptions<InfoOptions> options)
    {
        _options = options.Value;
    }
    
    [HttpGet]
    public ActionResult<UsageInfo> GetUsageInfo()
    {
        return new ActionResult<UsageInfo>(new UsageInfo(_options));
    }
}