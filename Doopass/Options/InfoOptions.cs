namespace Doopass.Options;

public class InfoOptions : IOptions
{
    public static string Position => "Info";
    public string ApiVersion { get; set; } = string.Empty;
    public string DocumentationUrl { get; set; } = string.Empty;
    public string RepositoryUrl { get; set; } = string.Empty;
    public string SupportEmail { get; set; } = string.Empty;
}