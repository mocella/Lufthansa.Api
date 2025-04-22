using System.Text;
using Lufthansa.Api.Models.Operations;
using Lufthansa.Api.Models.Shared;

namespace Lufthansa.Api.Services;

public interface IOperationService
{
    Task<ApiResponse<CustomerFlightResponse, ProcessingErrorResponse>> GetCustomerFlightInfo(
        string flightNumber,
        DateTime departureDate,
        int? limit = null,
        int? offset = null);

    Task<ApiResponse<CustomerFlightByRouteResponse, ProcessingErrorResponse>> GetCustomerFlightByRoute(
        string origin,
        string destination,
        DateTime departureDate,
        int? limit = null,
        int? offset = null);

    Task<ApiResponse<CustomerFlightAtArrivalResponse, ProcessingErrorResponse>> GetCustomerFlightAtArrival(
        string airportCode,
        DateTime arrivalDateTime,
        int? limit = null,
        int? offset = null);

    Task<ApiResponse<CustomerFlightAtDepartureResponse, ProcessingErrorResponse>>
        GetCustomerFlightAtDeparture(
            string airportCode,
            DateTime departureDateTime,
            int? limit = null,
            int? offset = null);

    Task<ApiResponse<FlightScheduleResponse, ProcessingErrorResponse>> GetFlightSchedule(
        string origin,
        string destination,
        DateTime departureDate,
        TimeOnly? departureTime = null,
        bool? directFlightsOnly = null,
        int? limit = null,
        int? offset = null);
}

public class OperationService(IHttpClientFactory httpClientFactory) : BaseService(httpClientFactory), IOperationService
{
    public async Task<ApiResponse<CustomerFlightResponse, ProcessingErrorResponse>> GetCustomerFlightInfo(
        string flightNumber,
        DateTime departureDate,
        int? limit = null,
        int? offset = null)
    {
        var options = new StringBuilder();
        var requestUri = $"operations/customerflightinformation/{flightNumber}/{departureDate:yyyy-MM-dd}";

        if (limit != null) options.Append($"limit={limit}&");
        if (offset != null) options.Append($"offset={offset}&");

        if (options.Length > 0) requestUri += $"?{options.ToString().TrimEnd('&')}";

        var response = await HttpClient.GetAsync(requestUri);
        return await ProcessApiResponseAsync<CustomerFlightResponse, ProcessingErrorResponse>(response);
    }

    public async Task<ApiResponse<CustomerFlightByRouteResponse, ProcessingErrorResponse>> GetCustomerFlightByRoute(
        string origin,
        string destination,
        DateTime departureDate,
        int? limit = null,
        int? offset = null)
    {
        var options = new StringBuilder();
        var requestUri =
            $"operations/customerflightinformation/route/{origin}/{destination}/{departureDate:yyyy-MM-dd}";

        if (limit != null) options.Append($"limit={limit}&");
        if (offset != null) options.Append($"offset={offset}&");

        if (options.Length > 0) requestUri += $"?{options.ToString().TrimEnd('&')}";

        var response = await HttpClient.GetAsync(requestUri);
        return await ProcessApiResponseAsync<CustomerFlightByRouteResponse, ProcessingErrorResponse>(response);
    }

    public async Task<ApiResponse<CustomerFlightAtArrivalResponse, ProcessingErrorResponse>> GetCustomerFlightAtArrival(
        string airportCode,
        DateTime arrivalDateTime,
        int? limit = null,
        int? offset = null)
    {
        var options = new StringBuilder();
        var requestUri =
            $"operations/customerflightinformation/arrivals/{airportCode}/{arrivalDateTime:yyyy-MM-ddTHH:mm}";

        if (limit != null) options.Append($"limit={limit}&");
        if (offset != null) options.Append($"offset={offset}&");

        if (options.Length > 0) requestUri += $"?{options.ToString().TrimEnd('&')}";

        var response = await HttpClient.GetAsync(requestUri);
        return await ProcessApiResponseAsync<CustomerFlightAtArrivalResponse, ProcessingErrorResponse>(response);
    }

    public async Task<ApiResponse<CustomerFlightAtDepartureResponse, ProcessingErrorResponse>>
        GetCustomerFlightAtDeparture(
            string airportCode,
            DateTime departureDateTime,
            int? limit = null,
            int? offset = null)
    {
        var options = new StringBuilder();
        var requestUri =
            $"operations/customerflightinformation/departures/{airportCode}/{departureDateTime:yyyy-MM-ddTHH:mm}";

        if (limit != null) options.Append($"limit={limit}&");
        if (offset != null) options.Append($"offset={offset}&");

        if (options.Length > 0) requestUri += $"?{options.ToString().TrimEnd('&')}";

        var response = await HttpClient.GetAsync(requestUri);
        return await ProcessApiResponseAsync<CustomerFlightAtDepartureResponse, ProcessingErrorResponse>(response);
    }

    public async Task<ApiResponse<FlightScheduleResponse, ProcessingErrorResponse>> GetFlightSchedule(
        string origin,
        string destination,
        DateTime departureDate,
        TimeOnly? departureTime = null,
        bool? directFlightsOnly = null,
        int? limit = null,
        int? offset = null)
    {
        var options = new StringBuilder();
        var requestUri =
            $"operations/schedules/{origin}/{destination}/{departureDate:yyyy-MM-dd}{(departureTime != null ? $"T{departureTime:hh\\:mm}" : string.Empty)}";

        if (directFlightsOnly != null) options.Append($"directFlights={(directFlightsOnly.Value ? 1 : 0)}&");
        if (limit != null) options.Append($"limit={limit}&");
        if (offset != null) options.Append($"offset={offset}&");

        if (options.Length > 0) requestUri += $"?{options.ToString().TrimEnd('&')}";

        var response = await HttpClient.GetAsync(requestUri);
        return await ProcessApiResponseAsync<FlightScheduleResponse, ProcessingErrorResponse>(response);
    }
}