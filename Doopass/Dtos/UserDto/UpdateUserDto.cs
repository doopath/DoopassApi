namespace Doopass.Dtos.UserDto;

public class UpdateUserDto : UserDto
{
    public override string? Name { get; set; }
    public override required int? Id { get; set; }
    public override string? Email { get; set; }
    public override bool IsEmailVerified { get; set; }
    public override required string Password { get; set; }
}