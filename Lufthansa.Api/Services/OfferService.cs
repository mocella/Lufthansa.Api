using Lufthansa.Api.Models.Offers;
using Lufthansa.Api.Models.Shared;

namespace Lufthansa.Api.Services;

public interface IOfferService
{
    Task<ApiResponse<SeatMapResponse, ProcessingErrorResponse>> GetSeatMap(string flightNumber,
        string origin,
        string destination,
        DateTime date,
        CabinClass cabinClass);

    Task<ApiResponse<LoungeResponse, ProcessingErrorResponse>> GetLoungeAsync(string location);
}

public class OfferService(IHttpClientFactory httpClientFactory) : BaseService(httpClientFactory), IOfferService
{
    public async Task<ApiResponse<SeatMapResponse, ProcessingErrorResponse>> GetSeatMap(string flightNumber,
        string origin,
        string destination,
        DateTime date,
        CabinClass cabinClass)
    {
        var response =
            await HttpClient.GetAsync(
                $"offers/seatmaps/{flightNumber}/{origin}/{destination}/{date:yyyy-M-d}/{(char)cabinClass}");
        return await ProcessApiResponseAsync<SeatMapResponse, ProcessingErrorResponse>(response);
    }

    public async Task<ApiResponse<LoungeResponse, ProcessingErrorResponse>> GetLoungeAsync(string location)
    {
        var response = await HttpClient.GetAsync($"offers/lounges/{location}");
        return await ProcessApiResponseAsync<LoungeResponse, ProcessingErrorResponse>(response);
    }
}