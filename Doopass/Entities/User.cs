using System.ComponentModel.DataAnnotations;

namespace Doopass.Entities;

public class User : IEntity
{
    [MaxLength(255)]
    public required string? Name { get; set; }
    
    public required int? Id { get; set; }
    
    [MaxLength(255)]
    public required string? Email { get; set; }
    
    public required bool IsEmailVerified { get; set; }
    public required string? Password { get; set; }
    
    public void UpdateOf(User from)
    {
        Name = from.Name ?? Name;
        Email = from.Email ?? Email;
        Password = from.Password ?? Password;
        
        if (Email != from.Email)
            IsEmailVerified = false;
        else
            IsEmailVerified &= from.IsEmailVerified;
    }
}