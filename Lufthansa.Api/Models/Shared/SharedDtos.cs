using System.Text.Json.Serialization;
using Lufthansa.Api.Models.JsonConverters;

namespace Lufthansa.Api.Models.Shared;

[JsonConverter(typeof(AirportsConverter))]
public class Airports
{
    public List<Airport>? AirportList { get; set; } = new();
}

public class Airport
{
    public string? AirportCode { get; set; }
    public AirportPosition? Position { get; set; }
    public string? CityCode { get; set; }
    public string? CountryCode { get; set; }
    public string? LocationType { get; set; }
    public Names? Names { get; set; }
    public string? UtcOffset { get; set; }
    public string? TimeZoneId { get; set; }
    public Distance? Distance { get; set; }
}

public class SeatAvailabilityResource
{
    public Flights? Flights { get; set; }
    public SeatDisplays? SeatDisplay { get; set; } = new();
    public SeatDetails[]? SeatDetails { get; set; }
    public Meta? Meta { get; set; }
}

[JsonConverter(typeof(SeatDisplaysConverter))]
public class SeatDisplays
{
    public List<SeatDisplay>? SeatDisplayList { get; set; } = new();
}

public class AirportPosition
{
    public Coordinate? Coordinate { get; set; }
}

public class Coordinate
{
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }
}

public class Distance
{
    public int? Value { get; set; }
    [JsonPropertyName("@UOM")] public string? UoM { get; set; }
}

[JsonConverter(typeof(NamesConverter))]
public class Names
{
    public List<Name> NameList { get; set; } = new();
}

public class Name
{
    [JsonPropertyName("@LanguageCode")] public string? LanguageCode { get; set; }

    [JsonPropertyName("$")] public string? LocalizedName { get; set; }
}

[JsonConverter(typeof(MetaLinkConverter))]
public class Meta
{
    [JsonPropertyName("@Version")] public string? Version { get; set; }

    public LinkContainer? Links { get; set; } = new();
    public int? TotalCount { get; set; }
}

public class LinkContainer
{
    public List<Link> Items { get; set; } = new();
}

public class Link
{
    [JsonPropertyName("@Href")] public string? Href { get; set; }

    [JsonPropertyName("@Rel")] public string? Rel { get; set; }
}

public class Terminal
{
    public string? Name { get; set; }
    public string? Gate { get; set; }
}

public class FlightInformation
{
    public Flights? Flights { get; set; }
    public Meta? Meta { get; set; }
}

[JsonConverter(typeof(FlightsConverter))]
public class Flights
{
    public List<Flight>? FlightList { get; set; } = new();
}

public class Flight
{
    public Departure? Departure { get; set; }
    public Arrival? Arrival { get; set; }
    public MarketingCarrier? MarketingCarrier { get; set; }
    public OperatingCarrier? OperatingCarrier { get; set; }
    public Equipment? Equipment { get; set; }
    public Details? Details { get; set; }
    public Status? Status { get; set; }
}

public class Status
{
    public string? Code { get; set; }
    public string? Description { get; set; }
}

public class Departure
{
    public string? AirportCode { get; set; }
    public ScheduledTimeLocal? ScheduledTimeLocal { get; set; }
    public Scheduled? Scheduled { get; set; }
    public Actual? Actual { get; set; }
    public Terminal? Terminal { get; set; }
    public Status? Status { get; set; }
}

public class Scheduled
{
    public string? Date { get; set; }
    public string? Time { get; set; }
}

public class Estimated
{
    public string? Date { get; set; }
    public string? Time { get; set; }
}

public class Actual
{
    public string? Date { get; set; }
    public string? Time { get; set; }
}

public class ScheduledTimeLocal
{
    public string? DateTime { get; set; }
}

public class OperatingCarrier
{
    [JsonPropertyName("@AirlineID")] public string? AirlineId { get; set; }
    public string? FlightNumber { get; set; }
}

public class Arrival
{
    public string? AirportCode { get; set; }
    public ScheduledTimeLocal? ScheduledTimeLocal { get; set; }
    public Scheduled? Scheduled { get; set; }
    public Estimated? Estimated { get; set; }
    public Actual? Actual { get; set; }
    public Terminal? Terminal { get; set; }
    public Status? Status { get; set; }
}

public class Details
{
    public Stops? Stops { get; set; }
    public string? DaysOfOperation { get; set; }
    public DatePeriod? DatePeriod { get; set; }
}

public class Stops
{
    public int? StopQuantity { get; set; }
}

public class DatePeriod
{
    public string? Effective { get; set; }
    public string? Expiration { get; set; }
}

public class MarketingCarrier
{
    public string? AirlineID { get; set; }
    public string? FlightNumber { get; set; }
}

public class Equipment
{
    public string? AircraftCode { get; set; }
    public OnBoardEquipment? OnBoardEquipment { get; set; }
}

public class OnBoardEquipment
{
    public bool? InflightEntertainment { get; set; }
    public Compartment[]? Compartment { get; set; }
}

public class Compartment
{
    public string? ClassCode { get; set; }
    public string? ClassDesc { get; set; }
    public bool? FlyNet { get; set; }
    public bool? SeatPower { get; set; }
    public bool? Usb { get; set; }
    public bool? LiveTv { get; set; }
}

public class SeatDisplay
{
    public Columns[]? Columns { get; set; }
    public Rows? Rows { get; set; }
    public CabinType? CabinType { get; set; }
    public Component[]? Component { get; set; }
}

public class Columns
{
    public string? Position { get; set; }
}

public class Rows
{
    public string? First { get; set; }
    public string? Last { get; set; }
}

public class CabinType
{
    public string? Code { get; set; }
}

public class Component
{
    public Locations? Locations { get; set; }
}

public class Locations
{
    public Location[]? Location { get; set; }
}

public class Location
{
    public Row? Row { get; set; }
    public Column? Column { get; set; }
    public LocationType? Type { get; set; }
}

public class Row
{
    public string? Position { get; set; }
    public Orientation? Orientation { get; set; }
    public string? Number { get; set; }
    public Characteristics? Characteristics { get; set; } = new();
}

public class Orientation
{
    public string? Code { get; set; }
}

[JsonConverter(typeof(ColumnConverter))]
public class Column
{
    public string? Code { get; set; }
    public Position? Position { get; set; }
}

public class Position
{
    public string? Code { get; set; }
}

public class LocationType
{
    public string? Code { get; set; }
}

public class SeatDetails
{
    public Location? Location { get; set; }
}

public class Characteristics
{
    [JsonConverter(typeof(CharacteristicCollectionConverter))]
    [JsonPropertyName("Characteristic")]
    public List<Characteristic>? CharacteristicList { get; set; } = new();
}

public class Characteristic
{
    public string? Code { get; set; }
}

public class LoungeResource
{
    public Lounges? Lounges { get; set; }
    public Meta? Meta { get; set; }
}

public class Lounges
{
    public Lounge[]? Lounge { get; set; }
}

public class Lounge
{
    public Names? Names { get; set; }
    public string? AirportCode { get; set; }
    public string? CityCode { get; set; }
    public LoungeLocations? Locations { get; set; }
    public OpeningHours? OpeningHours { get; set; }
    public Features? Features { get; set; }
    public MagazinesAndNews? MagazinesAndNews { get; set; }
}

public class LoungeLocations
{
    public LoungeLocation? Location { get; set; }
}

public class LoungeLocation
{
    [JsonPropertyName("@LanguageCode")] public string? LanguageCode { get; set; }

    [JsonPropertyName("$")] public string? LocalizedName { get; set; }
}

public class OpeningHours
{
    [JsonPropertyName("OpeningHours")] public OpeningHour? OpeningHour { get; set; }
}

public class OpeningHour
{
    [JsonPropertyName("@LanguageCode")] public string? LanguageCode { get; set; }

    [JsonPropertyName("$")] public string? LocalizedName { get; set; }
}

public class Features
{
    public bool? NonSmokingLounge { get; set; }
    public bool? Restrooms { get; set; }
    public bool? ShowerFacilities { get; set; }
    public bool? RelaxingRooms { get; set; }
    public bool? MeetingRooms { get; set; }
    public bool? OfficeUnits { get; set; }
    public bool? CopyMachine { get; set; }
    public bool? FaxMachine { get; set; }
    [JsonPropertyName("@MAMPrinter")] public bool? MamPrinter { get; set; }
    public bool? CreditCardTelephone { get; set; }
    public bool? FreeLocalTelephoneCalls { get; set; }

    [JsonPropertyName("@PCWithInternetAccess")]
    public bool? PcWithInternetAccess { get; set; }

    public bool? DataPortForLaptops { get; set; }
    [JsonPropertyName("@WLANFacility")] public bool? WlanFacility { get; set; }
    [JsonPropertyName("@TV")] public bool? Tv { get; set; }
}

public class MagazinesAndNews
{
    public bool? German { get; set; }
    public bool? International { get; set; }
    public bool? Local { get; set; }
}