using System.Security.Cryptography;
using System.Text;

namespace Doopass.Models;

public class PasswordHandler
{
    public virtual string Hash => GetHash();
    public virtual bool IsValid => Validate();
    public virtual string? ValidationMessage { get; private set; }
    
    protected readonly string Password;

    public PasswordHandler(string password)
    {
        Password = password;
    }

    protected virtual bool Validate()
    {
        if (Password.Length <= 0)
            ValidationMessage = "Length of the password must be greater than 0!";
        
        if (Password.Contains(' '))
            ValidationMessage = "Password must not contain spaces!";
        
        return ValidationMessage is null;
    }
    
    protected virtual string GetHash()
    {
        var builder = new StringBuilder();

        using (var hash = SHA256.Create())            
        {
            var enc = Encoding.UTF8;
            var result = hash.ComputeHash(enc.GetBytes(Password));

            foreach (var b in result)
                builder.Append(b.ToString("x2"));
        }

        return builder.ToString();
    }
}