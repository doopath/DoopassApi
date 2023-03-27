using System.Security.Cryptography;
using System.Text;

namespace Doopass.Models;

public static class Hash
{
    public static string GenSecureHash(string hashingData)
    {
        return ToSecureHash(hashingData);
    }

    public static string GenHash(string hashingData)
    {
        return ToSHA256(hashingData);
    }

    public static bool CompareSecureHash(string sampleHash, string targetData)
    {
        return BCrypt.Net.BCrypt.EnhancedVerify(targetData, sampleHash);
    }

    public static bool CompareHash(string sampleHash, string targetData)
    {
        return sampleHash.Equals(ToSHA256(targetData));
    }

    private static string ToSecureHash(string hashingData)
    {
        return BCrypt.Net.BCrypt.EnhancedHashPassword(hashingData);
    }

    private static string ToSHA256(string hashingData)
    {
        var builder = new StringBuilder();
        var enc = Encoding.UTF8;
        var result = SHA256.HashData(enc.GetBytes(hashingData));

        foreach (var b in result)
            builder.Append(b.ToString("x2"));

        return builder.ToString();
    }
}