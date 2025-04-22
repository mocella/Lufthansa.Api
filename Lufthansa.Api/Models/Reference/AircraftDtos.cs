using Lufthansa.Api.Models.Shared;

namespace Lufthansa.Api.Models.Reference;

public class AircraftResponse
{
    public AircraftResource? AircraftResource { get; set; }
}

public class AircraftResource
{
    public AircraftSummaries? AircraftSummaries { get; set; }
    public Meta? Meta { get; set; }
}

public class AircraftSummaries
{
    public AircraftSummary[]? AircraftSummary { get; set; }
}

public class AircraftSummary
{
    public string? AircraftCode { get; set; }
    public Names? Names { get; set; }
    public string? AirlineEquipCode { get; set; }
}