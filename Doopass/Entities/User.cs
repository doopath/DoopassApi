using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Doopass.Entities;

[Table("Users")]
public class User : IEntity
{
    [MaxLength(255)] public required string? Name { get; set; }

    [MaxLength(255)] [EmailAddress] public required string? Email { get; set; }

    public required bool IsEmailVerified { get; set; }

    [StringLength(64)] public required string? Password { get; set; }

    public int? StoreId { get; set; }

    [ForeignKey("StoreId")] public required Store? Store { get; set; }

    public required List<int>? BackupsIds { get; set; }

    [Key] public required int? Id { get; set; }

    public void UpdateOf(User from)
    {
        Name = from.Name ?? Name;
        Email = from.Email ?? Email;
        Password = from.Password ?? Password;
        Store = from.Store ?? Store;
        BackupsIds = from.BackupsIds ?? BackupsIds;

        if (Email != from.Email)
            IsEmailVerified = false;
        else
            IsEmailVerified &= from.IsEmailVerified;
    }
}