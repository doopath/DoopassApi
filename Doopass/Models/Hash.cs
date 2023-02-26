using System.Security.Cryptography;
using System.Text;

namespace Doopass.Models;

public class Hash
{
    protected readonly string HashingData;

    public Hash(string hashingData)
    {
        HashingData = hashingData;
    }

    public override string ToString()
        => ToSHA256();
    
    protected virtual string ToSHA256()
    {
        var builder = new StringBuilder();

        using (var hash = SHA256.Create())
        {
            var enc = Encoding.UTF8;
            var result = hash.ComputeHash(enc.GetBytes(HashingData));

            foreach (var b in result)
                builder.Append(b.ToString("x2"));
        }

        return builder.ToString();
    }
}