namespace Doopass.Models;

public class PasswordHandler
{
    protected readonly string Password;

    public PasswordHandler(string password)
    {
        Password = password;
    }

    public virtual string Hash => Doopass.Models.Hash.GenSecureHash(Password);
    public virtual bool IsValid => Validate();
    public virtual string? ValidationMessage { get; private set; }

    public static bool CompareHash(string sampleHash, string targetData)
        => Models.Hash.CompareSecureHash(sampleHash, targetData);

    protected virtual bool Validate()
    {
        if (Password.Length <= 0)
            ValidationMessage = "Length of the password must be greater than 0!";

        if (Password.Contains(' '))
            ValidationMessage = "Password must not contain spaces!";

        return ValidationMessage is null;
    }
}