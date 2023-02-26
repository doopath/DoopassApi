using Doopass.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Doopass.EntityTypeConfigurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");
            
        builder.HasIndex(user => user.Id).IsUnique();
        builder
            .HasOne<Store>(user => user.Store)
            .WithOne(store => store.User)
            .HasForeignKey<Store>(store => store.UserId);
        
        builder.HasKey(user => user.Id);
        
        builder.Property(user => user.Name).IsRequired();
        builder.Property(user => user.Email).IsRequired();
        builder.Property(user => user.Id).IsRequired();
        builder.Property(user => user.IsEmailVerified).IsRequired();
        builder.Property(user => user.Password).IsRequired();

        builder.Property(user => user.Name).HasMaxLength(255);
        builder.Property(user => user.Email).HasMaxLength(255);
        builder.Property(user => user.Password).IsFixedLength();
    }
}