using BookingWebApiV1.Database;

namespace BookingWebApiV1.Services.ApiKeyService;

public class ApiKeyService : IApiKeyService
{
    private IDatabaseContext DatabaseContext { get; }

    public ApiKeyService(IDatabaseContext databaseContext)
    {
        DatabaseContext = databaseContext;
    }
    
    public async Task<bool> IsValidApiKey(string? masterArduinoId, string? apiKey)
    {
        var dbResult = await DatabaseContext.GetMasterArduino(masterArduinoId, apiKey);

        return dbResult.MasterArduinoId == masterArduinoId && dbResult.ApiKey == apiKey;
    }
}