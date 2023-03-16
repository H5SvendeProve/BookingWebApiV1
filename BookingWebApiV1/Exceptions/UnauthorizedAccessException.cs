namespace BookingWebApiV1.Exceptions;

public class UnauthorizedAccessException : Exception
{
    public UnauthorizedAccessException(string message) : base(message)
    {
        
    }
}