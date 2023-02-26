using Doopass.Entities;
using Doopass.EntityTypeConfigurations;
using Doopass.Options;
using Microsoft.EntityFrameworkCore;

namespace Doopass;

public sealed class DoopassContext : DbContext
{
    private readonly DbOptions _options;

    public DoopassContext(DbOptions options)
    {
        _options = options;
        Database.EnsureCreated();
    }

    public DbSet<User>? Users { get; set; }
    public DbSet<Store>? Stores { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        new UserConfiguration().Configure(modelBuilder.Entity<User>());
        new StoreConfiguration().Configure(modelBuilder.Entity<Store>());
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