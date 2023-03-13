namespace BookingWebApiV1.Authentication;

public class JwtOptions
{
    public string Issuer { get; init; }

    public string Audience { get; init; }

    public string Secretkey { get; init; }
}