namespace Doopass.Options;

public class InfoOptions : IOptions
{
    public static string Position => "Info";
    public required string ApiVersion { get; set; } = string.Empty;
    public required string DocumentationUrl { get; set; } = string.Empty;
    public required string RepositoryUrl { get; set; } = string.Empty;
    public required string SupportEmail { get; set; } = string.Empty;
}