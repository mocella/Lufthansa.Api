using System.Text.Json;
using System.Text.Json.Serialization;
using Lufthansa.Api.Models.Shared;

namespace Lufthansa.Api.Models.JsonConverters;

public class FlightPropertyConverter : JsonConverter<List<Flight>>
{
    public override List<Flight> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var flights = new List<Flight>();

        if (reader.TokenType == JsonTokenType.StartObject)
        {
            // Single flight case
            var flight = JsonSerializer.Deserialize<Flight>(ref reader, options);
            if (flight != null) flights.Add(flight);
        }
        else if (reader.TokenType == JsonTokenType.StartArray)
        {
            // Array of flights case
            while (reader.Read() && reader.TokenType != JsonTokenType.EndArray)
                if (reader.TokenType == JsonTokenType.StartObject)
                {
                    var flight = JsonSerializer.Deserialize<Flight>(ref reader, options);
                    if (flight != null) flights.Add(flight);
                }
        }

        return flights;
    }

    public override void Write(Utf8JsonWriter writer, List<Flight> value, JsonSerializerOptions options)
    {
        if (value.Count == 1)
        {
            // Single flight case
            JsonSerializer.Serialize(writer, value[0], options);
        }
        else
        {
            // Array of flights case
            writer.WriteStartArray();
            foreach (var flight in value) JsonSerializer.Serialize(writer, flight, options);
            writer.WriteEndArray();
        }
    }
}