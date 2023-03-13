namespace BookingWebApiV1.Services.ApiKeyService;

public interface IApiKeyService
{
    Task<bool> IsValidApiKey(string? masterArduinoId, string? apiKey);
}