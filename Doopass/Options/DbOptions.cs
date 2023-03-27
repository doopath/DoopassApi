namespace Doopass.Options;

public class DbOptions : IOptions
{
    public static string Position => "Database";
    public required int Port { get; set; } = default;
    public required string Host { get; set; } = string.Empty;

    public required string Password { get; init; } =
        Environment.GetEnvironmentVariable("DOOPASS_DB_PASSWORD") ?? "root";

    public required string DbName { get; set; } = string.Empty;
    public required string Username { get; set; } = string.Empty;
}