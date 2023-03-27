using System.ComponentModel.DataAnnotations.Schema;

namespace Doopass.Entities;

[Table("Users")]
public class User : IEntity
{
    public required string? Name { get; set; }

    public required string? Email { get; set; }

    public required bool IsEmailVerified { get; set; }

    public required string Password { get; set; }

    public Store? Store { get; set; }
    
    public required int? StoreId { get; set; }

    public required List<int>? BackupsIds { get; set; }

    public required int? Id { get; set; }

    public void UpdateOf(User from)
    {
        Name = from.Name ?? Name;
        Email = from.Email ?? Email;
        Password = from.Password;
        Store = from.Store ?? Store;
        BackupsIds = from.BackupsIds ?? BackupsIds;

        if (Email != from.Email)
            IsEmailVerified = false;
        else
            IsEmailVerified &= from.IsEmailVerified;
    }
}