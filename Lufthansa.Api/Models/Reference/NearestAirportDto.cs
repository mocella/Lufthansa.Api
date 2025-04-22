using Lufthansa.Api.Models.Shared;

namespace Lufthansa.Api.Models.Reference;

public class NearestAirportResponse
{
    public NearestAirportResource? NearestAirportResource { get; set; }
}

public class NearestAirportResource
{
    public Airports? Airports { get; set; }
    public Meta? Meta { get; set; }
}