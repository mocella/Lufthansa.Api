using System.Text.Json;
using Lufthansa.Api.Models.Shared;

namespace Lufthansa.Api.Services;

public class BaseService(IHttpClientFactory httpClientFactory)
{
    protected HttpClient HttpClient { get; set; } = httpClientFactory.CreateClient("Lufthansa");

    protected async Task<ApiResponse<TSuccess, TError>> ProcessApiResponseAsync<TSuccess, TError>(
        HttpResponseMessage response)
    {
        var content = await response.Content.ReadAsStringAsync();

        if (response.IsSuccessStatusCode)
        {
            var successResponse = JsonSerializer.Deserialize<TSuccess>(content);
            return ApiResponse<TSuccess, TError>.FromSuccess(successResponse!);
        }

        var errorResponse = JsonSerializer.Deserialize<TError>(content);
        return ApiResponse<TSuccess, TError>.FromError(errorResponse!);
    }
}