using Doopass.Entities;
using Doopass.Options;
using Microsoft.EntityFrameworkCore;

namespace Doopass;

public sealed class DoopassContext : DbContext
{
    public DbSet<User>? Users { get; set; }
    private readonly DbOptions _options;

    public DoopassContext(DbOptions options)
    {
        _options = options;
    }


    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(
            $"host={_options.Host};" +
            $"port={_options.Port};" +
            $"database={_options.DbName};" +
            $"username={_options.Username};" +
            $"password={_options.Password}");
    }
}