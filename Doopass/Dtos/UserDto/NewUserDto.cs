namespace Doopass.Dtos.UserDto;

public class NewUserDto : UserDto
{
    public override required string? Name { get; set; }
    public override int? Id { get; set; }
    public override required string? Email { get; set; }
    public override bool IsEmailVerified { get; set; }
    public override required string? Password { get; set; }
}