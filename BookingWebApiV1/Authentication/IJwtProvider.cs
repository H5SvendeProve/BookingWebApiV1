namespace BookingWebApiV1.Authentication;

public interface IJwtProvider
{
    Task<string> GenerateNewToken(string username);
    Task<bool> IsTokenValid(string token);
}