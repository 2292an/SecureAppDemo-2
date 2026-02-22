namespace SecureAppDemo.Exceptions;

public class BadRequestResponseException : Exception
{
    public BadRequestResponseException(string message) : base(message)
    {
    }
}