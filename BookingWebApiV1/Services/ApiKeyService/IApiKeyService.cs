namespace BookingWebApiV1.Services.ApiKeyService;

public interface IApiKeyService
{
    Task<bool> ValidateApiKey(string? masterArduinoId, string? apiKey);
}