using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Doopass.Entities;

[Table("Stores")]
public class Store : IEntity
{
    public int? UserId { get; set; }
    public required User? User { get; set; }

    public required string FilePath { get; set; }

    public required string LastUpdateDate { get; set; }

    [Key] public required int? Id { get; set; }
}