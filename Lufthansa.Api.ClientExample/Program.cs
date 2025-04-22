using Lufthansa.Api.Configuration;
using Lufthansa.Api.Middleware;
using Lufthansa.Api.Models.Offers;
using Lufthansa.Api.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateApplicationBuilder(args);

// Add UserSecrets to the configuration
builder.Configuration.AddUserSecrets<Program>();

builder.Services.Configure<LufthansaConfig>(builder.Configuration.GetSection("Lufthansa"));

builder.Services.AddSingleton<ITokenService, TokenService>();
builder.Services.AddTransient<TokenHandler>();

var lufthansaConfig = builder.Configuration.GetSection("Lufthansa").Get<LufthansaConfig>()!;
var clientBaseAddress = new Uri(lufthansaConfig.BaseUrl!);

// Register base HttpClient without handler for token service
builder.Services.AddHttpClient("LufthansaBase", (serviceProvider, client) =>
{
    client.BaseAddress = clientBaseAddress;
    client.DefaultRequestHeaders.Add("Accept", "application/json");
});

// Register main client with the handler
builder.Services.AddHttpClient("Lufthansa", (serviceProvider, client) =>
{
    client.BaseAddress = clientBaseAddress;
    client.DefaultRequestHeaders.Add("Accept", "application/json");
}).AddHttpMessageHandler<TokenHandler>();

// NOTE: normally would use Scrutor to register these but for simplicity inline here:
builder.Services.AddScoped<IOfferService, OfferService>();
builder.Services.AddScoped<IOperationService, OperationService>();
builder.Services.AddScoped<IReferenceService, ReferenceService>();

var app = builder.Build();

// Test harness to verify the services are working
using var scope = app.Services.CreateScope();

var referenceService = scope.ServiceProvider.GetRequiredService<IReferenceService>();

// Countries - get-all
var countries = await referenceService.GetCountries();
Console.Out.WriteLine($"Got {countries.Success!.CountryResource!.Countries!.CountryList!.Count} countries");

// Countries - get-by-code
var country =
    await referenceService.GetCountries(countries.Success!.CountryResource.Countries.CountryList[0].CountryCode);
Console.Out.WriteLine($"Got {country.Success!.CountryResource!.Countries!.CountryList![0].CountryCode}");

// Countries - get-by-code for non-existent
country = await referenceService.GetCountries("asdf");
Console.Out.WriteLine($"Got Error Description: {country.Error!.ProcessingErrors!.ProcessingError!.Description}");

// Cities - get-all
var cities = await referenceService.GetCities();
Console.Out.WriteLine($"Got {cities.Success!.CityResource!.Cities!.CityList!.Count} cities");

// Cities - get-by-code
var city = await referenceService.GetCities(cities.Success!.CityResource.Cities.CityList[0].CityCode);
Console.Out.WriteLine($"Got {city.Success!.CityResource!.Cities!.CityList![0].CityCode}");

// Cities - get-by-code for non-existent
city = await referenceService.GetCities("asdf");
Console.Out.WriteLine($"Got Error Description: {city.Error!.ProcessingErrors!.ProcessingError!.Description}");

// Airports - get-all
var airports = await referenceService.GetAirports();
Console.Out.WriteLine($"Got {airports.Success!.AirportResource!.Airports!.AirportList!.Count} airports");

// Airports - get-by-code
var airport =
    await referenceService.GetAirports(airports.Success!.AirportResource.Airports.AirportList[0].AirportCode);
Console.Out.WriteLine($"Got {airport.Success!.AirportResource!.Airports!.AirportList![0].AirportCode}");

// Airports - get-by-code for non-existent
airport = await referenceService.GetAirports("asdf");
Console.Out.WriteLine($"Got Error {airport.Error!.ProcessingErrors!.ProcessingError!.Description}");

// Nearest Airports - get with valid coordinates with nearby airports
var nearestAirports = await referenceService.GetNearestAirports(51.5111f, -0.14223123f);
Console.Out.WriteLine(
    $"Got {nearestAirports.Success!.NearestAirportResource!.Airports!.AirportList!.Count} nearest airports");

// Nearest Airports - Error case with invalid coordinates
nearestAirports = await referenceService.GetNearestAirports(-1251.5111f, -0.14223123f);
Console.Out.WriteLine($"Got Error {nearestAirports.Error!.ProcessingErrors!.ProcessingError!.Description}");

// Airlines - get-all
var airlines = await referenceService.GetAirlines();
Console.Out.WriteLine($"Got {airlines.Success!.AirlineResource!.Airlines!.AirlineList!.Count} airlines");

// Airlines - get-by-code
var airline =
    await referenceService.GetAirlines(airlines.Success!.AirlineResource.Airlines.AirlineList[0].AirlineId);
Console.Out.WriteLine($"Got {airline.Success!.AirlineResource!.Airlines!.AirlineList![0].AirlineId}");

// Airlines - get-by-code for non-existent
airline = await referenceService.GetAirlines("asdf");
Console.Out.WriteLine($"Got {airline.Error!.ProcessingErrors!.ProcessingError!.Description}");


var operationService = scope.ServiceProvider.GetRequiredService<IOperationService>();

// Airlines - get-by-flight-number
var customerFlightInfo = await operationService.GetCustomerFlightInfo("LX8", DateTime.Now);
Console.Out.WriteLine(
    $"Got {customerFlightInfo.Success!.FlightInformation!.Flights!.FlightList![0].OperatingCarrier!.AirlineId}");

// Airlines - get-by-flight-number for non-existent
customerFlightInfo = await operationService.GetCustomerFlightInfo("asdf", DateTime.Now);
Console.Out.WriteLine($"Got {customerFlightInfo.Error!.ProcessingErrors!.ProcessingError!.Description}");

// Airlines - get-by-route
var flightByRoute = await operationService.GetCustomerFlightByRoute("FRA", "JFK", DateTime.Now);
Console.Out.WriteLine(
    $"Got {flightByRoute.Success!.FlightInformation!.Flights!.FlightList![0].OperatingCarrier!.AirlineId}");

// Airlines - get-by-route for non-existent
flightByRoute = await operationService.GetCustomerFlightByRoute("XXX", "YYY", DateTime.Now);
Console.Out.WriteLine($"Got {flightByRoute.Error!.ProcessingErrors!.ProcessingError!.Description}");

// Airlines - get-by-arrival
var flightAtArrival = await operationService.GetCustomerFlightAtArrival("FRA", DateTime.Now);
Console.Out.WriteLine(
    $"Got {flightAtArrival.Success!.FlightInformation!.Flights!.FlightList![0].OperatingCarrier!.AirlineId}");

// Airlines - get-by-arrival for non-existent
flightAtArrival = await operationService.GetCustomerFlightAtArrival("XXX", DateTime.Now);
Console.Out.WriteLine($"Got {flightAtArrival.Error!.ProcessingErrors!.ProcessingError!.Description}");

// Airlines - get-by-departure
var flightAtDeparture = await operationService.GetCustomerFlightAtDeparture("FRA", DateTime.Now);
Console.Out.WriteLine(
    $"Got {flightAtDeparture.Success!.FlightInformation!.Flights!.FlightList![0].OperatingCarrier!.AirlineId}");

// Airlines - get-by-departure for non-existent
flightAtDeparture = await operationService.GetCustomerFlightAtDeparture("XXX", DateTime.Now);
Console.Out.WriteLine($"Got {flightAtDeparture.Error!.ProcessingErrors!.ProcessingError!.Description}");

// Airlines - get-flight-schedule with full date-time provided
var flightSchedule =
    await operationService.GetFlightSchedule("FRA", "JFK", DateTime.Now, TimeOnly.FromDateTime(DateTime.Now));
Console.Out.WriteLine(
    $"Got {flightSchedule.Success!.ScheduleResource!.Schedule![0].Flights![0].Departure!.AirportCode}");

// Airlines - get-flight-schedule with date only provided
flightSchedule = await operationService.GetFlightSchedule("FRA", "JFK", DateTime.Now);
Console.Out.WriteLine(
    $"Got {flightSchedule.Success!.ScheduleResource!.Schedule![0].Flights![0].Departure!.AirportCode}");

// Airlines - get-flight-schedule for non-existent
flightSchedule = await operationService.GetFlightSchedule("XXX", "YYY", DateTime.Now);
Console.Out.WriteLine($"Got {flightSchedule.Error!.ProcessingErrors!.ProcessingError!.Description}");


var offerService = scope.ServiceProvider.GetRequiredService<IOfferService>();

// SeatMap - get-seat-map
var seatMap = await offerService.GetSeatMap("LX8", "ZRH", "ORD", DateTime.Now, CabinClass.Business);
Console.Out.WriteLine($"Got {seatMap.Success!.SeatAvailabilityResource!.SeatDetails![0].Location!.Column!.Code}");

// SeatMap - get-seat-map for non-existent
seatMap = await offerService.GetSeatMap("ZZZ", "XXX", "YYY", DateTime.Now, CabinClass.Business);
Console.Out.WriteLine($"Got {seatMap.Error!.ProcessingErrors!.ProcessingError!.Description}");

// Lounge - get-lounge
var lounge = await offerService.GetLoungeAsync("FRA");
Console.Out.WriteLine($"Got {lounge.Success!.LoungeResource!.Lounges!.Lounge![0].AirportCode}");

// Lounge - get-lounge for non-existent
lounge = await offerService.GetLoungeAsync("XXX");
Console.Out.WriteLine($"Got {lounge.Error!.ProcessingErrors!.ProcessingError!.Description}");