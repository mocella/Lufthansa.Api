using System.Text.Json.Serialization;
using Lufthansa.Api.Models.JsonConverters;
using Lufthansa.Api.Models.Shared;

namespace Lufthansa.Api.Models.Reference;

public class CityResponse
{
    public CityResource? CityResource { get; set; }
}

public class CityResource
{
    public Cities? Cities { get; set; }
    public Meta? Meta { get; set; }
}

[JsonConverter(typeof(CitiesConverter))]
public class Cities
{
    public List<City>? CityList { get; set; } = new();
}

public class City
{
    public string? CityCode { get; set; }
    public string? CountryCode { get; set; }
    public Names? Names { get; set; }
    public string? UtcOffset { get; set; }
    public string? TimeZoneId { get; set; }
    public Airports? Airports { get; set; }
}