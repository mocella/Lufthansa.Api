using System.Text;
using System.Text.Json;
using Lufthansa.Api.Configuration;
using Lufthansa.Api.Models.Shared;
using Microsoft.Extensions.Options;

namespace Lufthansa.Api.Services;

public interface ITokenService
{
    Task<string> GetTokenAsync();
}

public class TokenService(
    IOptions<LufthansaConfig> lufthansaConfigOptions,
    IHttpClientFactory httpClientFactory)
    : ITokenService
{
    private readonly HttpClient _httpClient = httpClientFactory.CreateClient("LufthansaBase");
    private readonly Lock _lock = new();
    private readonly LufthansaConfig _lufthansaConfig = lufthansaConfigOptions.Value;
    private TokenResponse? _cachedToken;
    private DateTime? _tokenExpiration;

    public async Task<string> GetTokenAsync()
    {
        // Check if token is valid
        if (_cachedToken != null && DateTime.UtcNow < _tokenExpiration) return _cachedToken.AccessToken!;

        // Lock to ensure only one thread refreshes the token
        lock (_lock)
        {
            if (_cachedToken != null && DateTime.UtcNow < _tokenExpiration) return _cachedToken.AccessToken!;
        }

        // Refresh the token
        var route = "oauth/token";
        var content = new StringContent(
            $"client_id={_lufthansaConfig.ClientId}&client_secret={_lufthansaConfig.ClientSecret}&grant_type=client_credentials",
            Encoding.UTF8,
            "application/x-www-form-urlencoded"
        );

        var response = await _httpClient.PostAsync(route, content);

        if (!response.IsSuccessStatusCode)
            throw new HttpRequestException($"Failed to retrieve token. Status code: {response.StatusCode}");

        var responseBody = await response.Content.ReadAsStringAsync();
        var tokenResponse = JsonSerializer.Deserialize<TokenResponse>(responseBody);

        if (tokenResponse == null || string.IsNullOrEmpty(tokenResponse.AccessToken))
            throw new InvalidOperationException("Token response is invalid.");

        // Cache the token and expiration
        lock (_lock)
        {
            _cachedToken = tokenResponse;
            _tokenExpiration =
                DateTime.UtcNow.AddSeconds(tokenResponse.ExpiresIn!.Value - 60); // Subtract 60 seconds as a buffer
        }

        return _cachedToken.AccessToken;
    }
}