using BookingWebApiV1.Utils;

namespace BookingWebApiV1.Logging;

public sealed class Logger : ILogger, IDisposable
{
    
    private static readonly object Lock = new();
    private readonly string LogDirectory = SystemUtil.GetRootPath + "/logs";
    private const string LogFileNameDateFormat = "dd_MM_yyyy";
    private StreamWriter infoLogWriter = null!;
    private StreamWriter errorLogWriter = null!;

    public Logger()
    {
        InitializeWriters();
    }
    

    public async Task LogMessage(LogType logType, string message)
    {
        lock (Lock)
        {
            try
            {
                if (logType == LogType.Info)
                {
                    infoLogWriter.WriteLine($"{DateTime.Now} : {message}");
                    infoLogWriter.Flush();
                }
                else
                {
                    errorLogWriter.WriteLine($"{DateTime.Now} : {message}");
                    errorLogWriter.Flush();
                }
            }
            finally
            {
                Monitor.Exit(Lock);
            }
        }

        await Task.CompletedTask;
    }

    private void InitializeWriters()
    {
        if (!Directory.Exists(LogDirectory))
        {
            Directory.CreateDirectory(LogDirectory);
        }

        var date = DateTime.Now.ToString(LogFileNameDateFormat);
        
        infoLogWriter = new StreamWriter(Path.Combine(LogDirectory, $"{date}_info.log"), true);
        
        errorLogWriter = new StreamWriter(Path.Combine(LogDirectory, $"{date}_error.log"), true);
        
    }

    public void Dispose()
    {
        infoLogWriter.Dispose();
        errorLogWriter.Dispose();
    }
}