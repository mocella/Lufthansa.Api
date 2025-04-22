using System.Text.Json.Serialization;
using Lufthansa.Api.Models.JsonConverters;
using Lufthansa.Api.Models.Shared;

namespace Lufthansa.Api.Models.Operations;

public class FlightScheduleResponse
{
    public ScheduleResource? ScheduleResource { get; set; }
}

public class ScheduleResource
{
    public Schedule[]? Schedule { get; set; }
    public Meta? Meta { get; set; }
}

public class Schedule
{
    public TotalJourney? TotalJourney { get; set; }

    [JsonConverter(typeof(FlightPropertyConverter))]
    [JsonPropertyName("Flight")]
    public List<Flight>? Flights { get; set; } = new();
}

public class TotalJourney
{
    public string? Duration { get; set; }
}