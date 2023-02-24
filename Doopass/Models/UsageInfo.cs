using Doopass.Options;

namespace Doopass.Models;

public class UsageInfo
{
    public string Time { get; }
    public string ApiVersion { get; }
    public string DocumentationUrl { get; }
    public string RepositoryUrl { get; }
    public string SupportEmail { get; }
    
    public UsageInfo(InfoOptions options)
    {
        Time = DateTime.Now.ToString();
        ApiVersion = options.ApiVersion;
        DocumentationUrl = options.DocumentationUrl;
        RepositoryUrl = options.RepositoryUrl;
        SupportEmail = options.SupportEmail;
    }
}