namespace Doopass.Exceptions;

public class PasswordValidationException : Exception
{
    public PasswordValidationException(string message) : base(message)
    { }
}