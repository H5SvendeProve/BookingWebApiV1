using System.Security.Cryptography;

namespace BookingWebApiV1.Authentication.ApiKey;

public static class ApiKeyGenerator
{
    public static string GenerateNewApiKey(int keyLength)
    {
        const string allowedChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
        var randomBytes = new byte[keyLength];
    
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(randomBytes);
        }
    
        var chars = new char[keyLength];
        var allowedCharCount = allowedChars.Length;

        for (var i = 0; i < keyLength; i++)
        {
            chars[i] = allowedChars[randomBytes[i] % allowedCharCount];
        }
    
        return new string(chars);
    }
}