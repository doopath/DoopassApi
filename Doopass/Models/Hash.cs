using System.Security.Cryptography;
using System.Text;

namespace Doopass.Models;
using BCrypt.Net;

public static class Hash
{
    public static string GenSecureHash(string hashingData)
        => ToSecureHash(hashingData);

    public static string GenHash(string hashingData)
        => ToSHA256(hashingData);

    public static bool CompareSecureHash(string sampleHash, string targetData)
        => BCrypt.EnhancedVerify(targetData, sampleHash);

    public static bool CompareHash(string sampleHash, string targetData)
        => sampleHash.Equals(ToSHA256(targetData));
    
    private static string ToSecureHash(string hashingData)
        => BCrypt.EnhancedHashPassword(hashingData);
    
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