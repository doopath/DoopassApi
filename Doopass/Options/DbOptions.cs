namespace Doopass.Options;

public class DbOptions : IOptions
{
    public static string Position => "Database";
    public int Port { get; set; } = default;
    public string Host { get; set; } = string.Empty;
    public string Password { get; init; } = Environment.GetEnvironmentVariable("DOOPASS_DB_PASSWORD") ?? "root";
    public string DbName { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
}