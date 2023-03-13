namespace BookingWebApiV1.Logging;

public interface ILogger
{
    Task LogMessage(LogType logType, string message);
}