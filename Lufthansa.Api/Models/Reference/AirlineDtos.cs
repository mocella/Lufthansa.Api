using System.Text.Json.Serialization;
using Lufthansa.Api.Models.JsonConverters;
using Lufthansa.Api.Models.Shared;

namespace Lufthansa.Api.Models.Reference;

public class AirlineResponse
{
    public AirlineResource? AirlineResource { get; set; }
}

public class AirlineResource
{
    public Airlines? Airlines { get; set; }
    public Meta? Meta { get; set; }
}

[JsonConverter(typeof(AirlinesConverter))]
public class Airlines
{
    public List<Airline>? AirlineList { get; set; } = new();
}

public class Airline
{
    [JsonPropertyName("@AirlineID")] public string? AirlineId { get; set; }
    [JsonPropertyName("@AirlineID_ICAO")] public string? AirlineIdIcao { get; set; }
    public Names? Names { get; set; }
}