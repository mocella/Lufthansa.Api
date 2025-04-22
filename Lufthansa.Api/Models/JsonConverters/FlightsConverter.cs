using System.Text.Json;
using System.Text.Json.Serialization;
using Lufthansa.Api.Models.Shared;

namespace Lufthansa.Api.Models.JsonConverters;

public class FlightsConverter : JsonConverter<Flights>
{
    public override Flights Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var flights = new Flights();

        if (reader.TokenType != JsonTokenType.StartObject) throw new JsonException("Expected start of object");

        while (reader.Read() && reader.TokenType != JsonTokenType.EndObject)
            if (reader.TokenType == JsonTokenType.PropertyName)
            {
                var propertyName = reader.GetString();
                reader.Read();

                if (propertyName!.Equals("Flight", StringComparison.OrdinalIgnoreCase))
                {
                    if (reader.TokenType == JsonTokenType.StartObject)
                    {
                        // Single flight case
                        var flight = JsonSerializer.Deserialize<Flight>(ref reader, options);
                        if (flight != null) flights.FlightList!.Add(flight);
                    }
                    else if (reader.TokenType == JsonTokenType.StartArray)
                    {
                        // Array of flights case
                        while (reader.Read() && reader.TokenType != JsonTokenType.EndArray)
                            if (reader.TokenType == JsonTokenType.StartObject)
                            {
                                var flight = JsonSerializer.Deserialize<Flight>(ref reader, options);
                                if (flight != null) flights.FlightList!.Add(flight);
                            }
                    }
                }
                else
                {
                    // Skip other properties
                    reader.Skip();
                }
            }

        return flights;
    }

    public override void Write(Utf8JsonWriter writer, Flights value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();

        writer.WritePropertyName("Flight");
        if (value.FlightList!.Count == 1)
        {
            // Single flight case
            JsonSerializer.Serialize(writer, value.FlightList[0], options);
        }
        else
        {
            // Array of flights case
            writer.WriteStartArray();
            foreach (var flight in value.FlightList) JsonSerializer.Serialize(writer, flight, options);
            writer.WriteEndArray();
        }

        writer.WriteEndObject();
    }
}