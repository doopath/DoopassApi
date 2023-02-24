using System.ComponentModel.DataAnnotations;

namespace Doopass.Entities;

public record User : IEntity
{
    [MaxLength(255)]
    public required string? Name { get; set; }
    
    public required int? Id { get; set; }
    
    [MaxLength(255)]
    public required string? Email { get; set; }
    
    public required bool IsEmailVerified { get; set; }
    public required string? Password { get; set; }
}