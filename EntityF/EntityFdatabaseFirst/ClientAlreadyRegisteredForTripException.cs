namespace EntityFdatabaseFirst;

public class ClientAlreadyRegisteredForTripException : Exception
{
    public ClientAlreadyRegisteredForTripException(string? message) : base(message)
    {
    }
}