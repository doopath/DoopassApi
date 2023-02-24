namespace Doopass.Exceptions;

public class EntityWasNotFoundException : Exception
{
    public EntityWasNotFoundException(string message) : base(message)
    { }
}