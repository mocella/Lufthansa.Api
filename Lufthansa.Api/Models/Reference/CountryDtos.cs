using System.Text.Json.Serialization;
using Lufthansa.Api.Models.JsonConverters;
using Lufthansa.Api.Models.Shared;

namespace Lufthansa.Api.Models.Reference;

public class CountryResponse
{
    public CountryResource? CountryResource { get; set; }
}

public class CountryResource
{
    public Countries? Countries { get; set; }

    public Meta? Meta { get; set; }
}

[JsonConverter(typeof(CountriesConverter))]
public class Countries
{
    public List<Country>? CountryList { get; set; } = new();
}

public class Country
{
    public string? CountryCode { get; set; }
    public Names? Names { get; set; }
}