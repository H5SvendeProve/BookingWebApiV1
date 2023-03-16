namespace BookingWebApiV1.Exceptions;

public class HeaderNotFoundException : Exception
{
    public HeaderNotFoundException(string message) : base(message)
    {
        
    }
}