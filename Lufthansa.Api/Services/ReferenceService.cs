using System.Text;
using Lufthansa.Api.Models.Reference;
using Lufthansa.Api.Models.Shared;

namespace Lufthansa.Api.Services;

public interface IReferenceService
{
    Task<ApiResponse<CountryResponse, ProcessingErrorResponse>> GetCountries(string? countryCode = null,
        string? languageCode = null,
        int? limit = null,
        int? offset = null);

    Task<ApiResponse<CityResponse, ProcessingErrorResponse>> GetCities(string? cityCode = null,
        string? languageCode = null,
        int? limit = null,
        int? offset = null);

    Task<ApiResponse<AirportResponse, ProcessingErrorResponse>> GetAirports(string? airportCode = null,
        string? languageCode = null,
        int? limit = null,
        int? offset = null,
        bool? lufthansaOnly = null,
        GroupEnum? group = null);

    Task<ApiResponse<NearestAirportResponse, ProcessingErrorResponse>> GetNearestAirports(float latitude,
        float longitude,
        string? languageCode = null);

    Task<ApiResponse<AirlineResponse, ProcessingErrorResponse>> GetAirlines(string? airlineCode = null,
        int? limit = null,
        int? offset = null);

    Task<ApiResponse<AircraftResponse, ProcessingErrorResponse>> GetAircraft(string? aircraftCode,
        int? limit,
        int? offset);
}

public class ReferenceService(IHttpClientFactory httpClientFactory) : BaseService(httpClientFactory), IReferenceService
{
    public async Task<ApiResponse<CountryResponse, ProcessingErrorResponse>> GetCountries(string? countryCode = null,
        string? languageCode = null,
        int? limit = null,
        int? offset = null)
    {
        var options = new StringBuilder();
        var requestUri = $"mds-references/countries/{countryCode}";

        if (!string.IsNullOrWhiteSpace(languageCode)) options.Append($"lang={languageCode}&");
        if (limit != null) options.Append($"limit={limit}&");
        if (offset != null) options.Append($"offset={offset}&");

        if (options.Length > 0) requestUri += $"?{options.ToString().TrimEnd('&')}";

        var response = await HttpClient.GetAsync(requestUri);
        return await ProcessApiResponseAsync<CountryResponse, ProcessingErrorResponse>(response);
    }

    public async Task<ApiResponse<CityResponse, ProcessingErrorResponse>> GetCities(string? cityCode = null,
        string? languageCode = null,
        int? limit = null,
        int? offset = null)
    {
        var options = new StringBuilder();
        var requestUri = $"mds-references/cities/{cityCode}";

        if (!string.IsNullOrWhiteSpace(languageCode)) options.Append($"lang={languageCode}&");
        if (limit != null) options.Append($"limit={limit}&");
        if (offset != null) options.Append($"offset={offset}&");

        if (options.Length > 0) requestUri += $"?{options.ToString().TrimEnd('&')}";

        var response = await HttpClient.GetAsync(requestUri);
        return await ProcessApiResponseAsync<CityResponse, ProcessingErrorResponse>(response);
    }

    public async Task<ApiResponse<AirportResponse, ProcessingErrorResponse>> GetAirports(string? airportCode = null,
        string? languageCode = null,
        int? limit = null,
        int? offset = null,
        bool? lufthansaOnly = null,
        GroupEnum? group = null)
    {
        var options = new StringBuilder();
        var requestUri = $"mds-references/airports/{airportCode}";

        if (!string.IsNullOrWhiteSpace(languageCode)) options.Append($"lang={languageCode}&");
        if (limit != null) options.Append($"limit={limit}&");
        if (offset != null) options.Append($"offset={offset}&");
        if (lufthansaOnly != null) options.Append($"LHoperated={lufthansaOnly}&");
        if (group != null) options.Append($"group={group.ToString()}&");

        if (options.Length > 0) requestUri += $"?{options.ToString().TrimEnd('&')}";

        var response = await HttpClient.GetAsync(requestUri);
        return await ProcessApiResponseAsync<AirportResponse, ProcessingErrorResponse>(response);
    }

    public async Task<ApiResponse<NearestAirportResponse, ProcessingErrorResponse>> GetNearestAirports(float latitude,
        float longitude,
        string? languageCode = null)
    {
        var options = new StringBuilder();
        var roundedLatitude = Math.Round(latitude, 3);
        var roundedLongitude = Math.Round(longitude, 3);
        var requestUri = $"mds-references/airports/nearest/{roundedLatitude},{roundedLongitude}";

        if (!string.IsNullOrWhiteSpace(languageCode)) options.Append($"lang={languageCode}&");

        if (options.Length > 0) requestUri += $"?{options.ToString().TrimEnd('&')}";

        var response = await HttpClient.GetAsync(requestUri);
        return await ProcessApiResponseAsync<NearestAirportResponse, ProcessingErrorResponse>(response);
    }

    public async Task<ApiResponse<AirlineResponse, ProcessingErrorResponse>> GetAirlines(string? airlineCode = null,
        int? limit = null,
        int? offset = null)
    {
        var options = new StringBuilder();
        var requestUri = $"mds-references/airlines/{airlineCode}";

        if (limit != null) options.Append($"limit={limit}&");
        if (offset != null) options.Append($"offset={offset}&");

        if (options.Length > 0) requestUri += $"?{options.ToString().TrimEnd('&')}";

        var response = await HttpClient.GetAsync(requestUri);
        return await ProcessApiResponseAsync<AirlineResponse, ProcessingErrorResponse>(response);
    }

    public async Task<ApiResponse<AircraftResponse, ProcessingErrorResponse>> GetAircraft(string? aircraftCode = null,
        int? limit = null,
        int? offset = null)
    {
        var options = new StringBuilder();
        var requestUri = $"mds-references/aircraft/{aircraftCode}";

        if (limit != null) options.Append($"limit={limit}&");
        if (offset != null) options.Append($"offset={offset}&");

        if (options.Length > 0) requestUri += $"?{options.ToString().TrimEnd('&')}";

        var response = await HttpClient.GetAsync(requestUri);
        return await ProcessApiResponseAsync<AircraftResponse, ProcessingErrorResponse>(response);
    }
}