using Doopass.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Doopass.EntityTypeConfigurations;

public class StoreConfiguration : IEntityTypeConfiguration<Store>
{
    public void Configure(EntityTypeBuilder<Store> builder)
    {
        builder.ToTable("Stores");
        builder.HasKey(store => store.Id);
        builder.HasIndex(store => store.Id).IsUnique();
        builder
            .HasOne<User>(store => store.User)
            .WithOne(user => user.Store)
            .IsRequired()
            .HasForeignKey<User>(user => user.StoreId);

        builder.Property(store => store.Id).IsRequired();
        builder.Property(store => store.UserId).IsRequired();
        builder.Property(store => store.FilePath).IsRequired();
        builder.Property(store => store.LastUpdateDate).IsRequired();

        builder.Property(store => store.FilePath).HasMaxLength(255);
        builder.Property(store => store.LastUpdateDate).IsFixedLength();
    }
}