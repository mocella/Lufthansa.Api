using System.Net.Http.Headers;
using Lufthansa.Api.Services;

namespace Lufthansa.Api.Middleware;

public class TokenHandler(ITokenService tokenService) : DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        // Obtain the token
        var token = await tokenService.GetTokenAsync();

        // Add the Authorization header
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        // Proceed with the request
        return await base.SendAsync(request, cancellationToken);
    }
}