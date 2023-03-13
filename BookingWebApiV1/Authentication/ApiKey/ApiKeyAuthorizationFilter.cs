﻿using BookingWebApiV1.Services.ApiKeyService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.Tokens;

namespace BookingWebApiV1.Authentication.ApiKey;

public class ApiKeyAuthorizationFilter : IAsyncAuthorizationFilter
{
    private IApiKeyService ApiKeyService { get; }

    public ApiKeyAuthorizationFilter(IApiKeyService apiKeyService)
    {
        ApiKeyService = apiKeyService;
    }
    // ||!context.HttpContext.Request.Headers.TryGetValue("X-MasterArduinoId-Id", out var masterArduinoHeader))
    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        StringValues masterArduino = string.Empty;
        
        if (!context.HttpContext.Request.Headers.TryGetValue("X-Api-Key", out var apiKey)
            ||!context.HttpContext.Request.Headers.TryGetValue("X-MasterArduinoId", out masterArduino))
        {

            if (apiKey.Count == 0)
            {
                context.Result = new UnauthorizedObjectResult("Api key in header X-Api-Key is missing");
                return;
            }
            

            if (masterArduino.Count == 0)
            { 
                context.Result = new UnauthorizedObjectResult("ArduinoMasterId in header X-MasterArduinoId-Id is missing");
                return;
            }
        }
        

        bool isValid = await ValidApiKey(masterArduino, apiKey);

        if (!isValid)
        {
            context.Result = new UnauthorizedObjectResult("API key is not valid");
        }
    }
    
    private async Task<bool> ValidApiKey(string? masterArduinoIdValue, string? apiKeyValue)
    {
        if (apiKeyValue.IsNullOrEmpty() || masterArduinoIdValue.IsNullOrEmpty())
        {
            return false;
        }
            
        return await ApiKeyService.IsValidApiKey(masterArduinoIdValue, apiKeyValue);
    }
}