using System.Globalization;
using Doopass.Options;

namespace Doopass.Models;

public class UsageInfo
{
    public UsageInfo(InfoOptions options)
    {
        Time = DateTime.Now.ToString(CultureInfo.InvariantCulture);
        ApiVersion = options.ApiVersion;
        DocumentationUrl = options.DocumentationUrl;
        RepositoryUrl = options.RepositoryUrl;
        SupportEmail = options.SupportEmail;
    }

    public string Time { get; }
    public string ApiVersion { get; }
    public string DocumentationUrl { get; }
    public string RepositoryUrl { get; }
    public string SupportEmail { get; }
}