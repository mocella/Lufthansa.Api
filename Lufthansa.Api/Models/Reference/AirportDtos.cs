using Lufthansa.Api.Models.Shared;

namespace Lufthansa.Api.Models.Reference;

public class AirportResponse
{
    public AirportResource? AirportResource { get; set; }
}

public class AirportResource
{
    public Airports? Airports { get; set; }
    public Meta? Meta { get; set; }
}