namespace BookingWebApiV1.Utils;

public static class SystemUtil
{
    public static readonly string GetRootPath =
        Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
}