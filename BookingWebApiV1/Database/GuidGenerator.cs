namespace BookingWebApiV1.Database;

public static class GuidGenerator
{
    public static string GenerateNewGuidString()
    {
        return  Guid.NewGuid().ToString();
    }
}