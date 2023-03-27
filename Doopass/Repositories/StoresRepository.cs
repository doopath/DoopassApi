using Doopass.Entities;
using Doopass.Options;
using Microsoft.Extensions.Options;

namespace Doopass.Repositories;

public class StoresRepository : EntityRepository<Store>
{
    public StoresRepository(IOptions<DbOptions> options) : base(options)
    {
    }
}