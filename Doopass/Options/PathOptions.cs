namespace Doopass.Options;

public class PathOptions : IOptions
{
    public static string Position => "Paths";
    public required string StoreStoragePath { get; set; } = string.Empty;
}